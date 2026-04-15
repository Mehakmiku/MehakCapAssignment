using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Storage.Blobs;

namespace BlobEncryptDecrypt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string tenantId = "";
            string clientId = "";
            string clientSecret = "";

            string keyVaultUrl = "https://keyvalutcg.vault.azure.net/";
            string keyName = "batch2key";

            string blobConnectionString = "";
            string containerName = "data";
            string blobName = "encryptedImage.bin";

            // 👇 ORIGINAL IMAGE NAME
            string inputBlobName = "ra.jpg";

            // 👇 AUTO GENERATED NAMES
            string fileName = Path.GetFileNameWithoutExtension(inputBlobName);
            string encryptedBlobName = fileName + "_encrypted.bin";
            string decryptedBlobName = fileName + "_decrypted.jpg";

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var keyClient = new KeyClient(new Uri(keyVaultUrl), credential);
            var key = await keyClient.GetKeyAsync(keyName);

            var cryptoClient = new CryptographyClient(key.Value.Id, credential);

            // ================= DOWNLOAD ORIGINAL =================
            BlobClient inputBlobClient = new BlobClient(
                blobConnectionString,
                containerName,
                inputBlobName
            );

            var inputDownload = await inputBlobClient.DownloadAsync();

            using MemoryStream inputStream = new MemoryStream();
            await inputDownload.Value.Content.CopyToAsync(inputStream);

            byte[] imageBytes = inputStream.ToArray();
            Console.WriteLine("✅ Image downloaded from Blob!");

            // ================= ENCRYPT =================
            using Aes aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();

            byte[] encryptedImage;
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(imageBytes);
                cs.Close();
                encryptedImage = ms.ToArray();
            }

            EncryptResult encryptedKey = await cryptoClient.EncryptAsync(
                EncryptionAlgorithm.RsaOaep,
                aes.Key);

            using MemoryStream finalStream = new MemoryStream();
            finalStream.Write(aes.IV);
            finalStream.Write(encryptedKey.Ciphertext);
            finalStream.Write(encryptedImage);

            byte[] finalData = finalStream.ToArray();

            // 📤 Upload encrypted file
            BlobClient encryptedBlobClient = new BlobClient(
                blobConnectionString,
                containerName,
                encryptedBlobName
            );

            using MemoryStream uploadStream = new MemoryStream(finalData);
            await encryptedBlobClient.UploadAsync(uploadStream, overwrite: true);

            Console.WriteLine("✅ Encrypted image uploaded!");

            // ================= DECRYPT =================
            var download = await encryptedBlobClient.DownloadAsync();

            using MemoryStream downloadStream = new MemoryStream();
            await download.Value.Content.CopyToAsync(downloadStream);

            byte[] blobData = downloadStream.ToArray();

            byte[] iv = new byte[16];
            Array.Copy(blobData, 0, iv, 0, 16);

            int keySize = 256;
            byte[] encryptedAesKey = new byte[keySize];
            Array.Copy(blobData, 16, encryptedAesKey, 0, keySize);

            byte[] encryptedImg = new byte[blobData.Length - 16 - keySize];
            Array.Copy(blobData, 16 + keySize, encryptedImg, 0, encryptedImg.Length);

            DecryptResult decryptedKey = await cryptoClient.DecryptAsync(
                EncryptionAlgorithm.RsaOaep,
                encryptedAesKey);

            using Aes aesDecrypt = Aes.Create();
            aesDecrypt.Key = decryptedKey.Plaintext;
            aesDecrypt.IV = iv;

            using MemoryStream msDecrypt = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(msDecrypt, aesDecrypt.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(encryptedImg);
                cs.Close();
            }

            byte[] finalImage = msDecrypt.ToArray();

            // 📤 Upload decrypted image
            BlobClient decryptedBlobClient = new BlobClient(
                blobConnectionString,
                containerName,
                decryptedBlobName
            );

            using MemoryStream decryptedStream = new MemoryStream(finalImage);
            await decryptedBlobClient.UploadAsync(decryptedStream, overwrite: true);

            Console.WriteLine("✅ Decrypted image uploaded to Blob!");
        }
    }
}

