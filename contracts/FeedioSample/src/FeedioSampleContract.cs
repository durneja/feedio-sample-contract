using System;
using System.ComponentModel;
using System.Numerics;

using Neo;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

namespace FeedioSample
{
    [DisplayName("Feedio.FeedioSampleContract")]
    [ManifestExtra("Author", "durneja")]
    [ManifestExtra("Email", "kinshuk.kar@gmail.com")]
    [ManifestExtra("Description", "Sample contract to retrieve price information from Feedio contract")]
    [ContractPermission("0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f", "*")]
    [ContractTrust("0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f")]
    public class FeedioSampleContract : SmartContract
    {

        private static Transaction Tx => (Transaction) Runtime.ScriptContainer;
        private static bool ValidateAddress(UInt160 address) => address.IsValid && !address.IsZero;

        protected const byte Prefix_Config = 0x01;
        protected const string Prefix_Config_Owner = "o";

        private static Boolean VerifyOwner()
        {
            StorageMap configData = new StorageMap(Storage.CurrentContext, Prefix_Config);
            UInt160 owner = (UInt160) configData.Get(Prefix_Config_Owner);
            if (Runtime.CheckWitness(owner)) {return true;}
            return false;
        }

        public static Map<string, object> GetTokenPrice() {
            
            ByteString latestPriceData = (ByteString) Contract.Call(ToScripthash("NUaWLwEL9bJZGmkpLesEmkkkmfDsFz1YGo"), "getLatestTokenPrice", CallFlags.All, (ByteString) "GAS");
            TokenPriceResponse priceResponse = (TokenPriceResponse)StdLib.JsonDeserialize(latestPriceData);
            Map<string, object> map = new Map<string, object>();
            map["name"] = priceResponse.name;
            map["value"] = priceResponse.value;
            map["decimals"] = priceResponse.decimals;

            return map;
        }

        public static void OnNEP11Payment(UInt160 from, UInt160 to, ByteString tokenId, object data) {

            return;
        }

        [DisplayName("_deploy")]
        public static void Deploy(object data, bool update)
        {
            if (!update)
            {
                initialize();
            }
        }

        private static void initialize() 
        {
            StorageMap configData = new StorageMap(Storage.CurrentContext, Prefix_Config);

            configData.Put(Prefix_Config_Owner, (UInt160) ToScripthash("Nc2JPKy62qCWWWSB6Ud6KL275u8yhWGTj5"));
        }

        public static void UpdateContract(ByteString nefFile, string manifest)
        {
            if (!VerifyOwner()) { throw new Exception("Not authorized for executing this method");}
            ContractManagement.Update(nefFile, manifest, null);
        }

        public static void Destroy()
        {
            if (!VerifyOwner()) { throw new Exception("Not authorized for executing this method");}
            ContractManagement.Destroy();
        }

        private static UInt160 ToScripthash(String address) {
            if ((address.ToByteArray())[0] == 0x4e)
            {
                var decoded = (byte[]) StdLib.Base58CheckDecode(address);
                var Scripthash = (UInt160)decoded.Last(20);
                return (Scripthash);
            }
            return null;
        }
    }

    public class TokenPriceResponse {
        public string name; //Token Name
        public BigInteger value; //Value of the token
        public BigInteger decimals; //Decimals

        public TokenPriceResponse(string tokenName, BigInteger tokenValue, BigInteger decimals) {
            this.name = tokenName;
            this.value = tokenValue;
            this.decimals = decimals;
        }
    }

}
