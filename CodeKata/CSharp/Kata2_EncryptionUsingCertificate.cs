using CodeKata.CSharp.Helpers.Kata2_EncryptionUsingCertificate;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

try
{
    X509Certificate2 myCert = Kata2_Helper.LoadCertificate();

    var publicKey = myCert.GetPublicKeyString();

    Console.WriteLine("Public Key: " + publicKey);

    string myText = "This is the text I wish to encrypt";
    Console.WriteLine("UNENCRYPTED: " + myText);

    string encrypted = Kata2_Helper.Encrypt(myCert, myText);

    //var encrypted = Kata2_Helper.EncryptWithPublicKey(myCert, myText, publicKey);

    Console.WriteLine("ENCRYPTED: " + encrypted);

    string decrypted = Kata2_Helper.Decrypt(myCert, encrypted);

    Console.WriteLine("DECRYPTED: " + decrypted);
}
catch (Exception e)
{
    Console.WriteLine("EXCEPTION: {0}", e.Message);
}