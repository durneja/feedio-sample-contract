# feedio-sample-contract
Sample contract to denote how the price feed apis can be consumed

This is a smart contract that is exclusively writted to depict how the smart contract interface for the Feedio Price feed can be utilized. 
The key aspects here are - Permissions, NFT Access, Actual Contract Call, Data Deserialization
Let's look at each of them in detail - 

## Permissions

The following directives need to be added to your calling smart contract to highlight the fact that you are invoking the Feedio Smart Contract from your particular smart contract code.

```
[ContractPermission("0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f", "getLatestTokenPrice")]
[ContractTrust("0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f")]
```

Here: **0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f** is the hash of the Feedio Primary Contract that is deployed on the N3 testnet.

Details of the contract can be viewed here - 
https://n3t4.neotube.io/contract/0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f or 
https://testnet.explorer.onegate.space/contractinfo/0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f

**getLatestTokenPrice** is the smart contract method that would be invoked. 

## NFT Access

Before starting to make the call, we have to ensure that the calling contract has an ownership of the Feedio Access Token NFT. 
The NFT is used as an access pass to retrieve data from the primary smart contract

Here: **0x8b6a955ef8026cecaf9393f1734bffe508ce42be** is the hash of the Feedio Primary Contract that is deployed on the N3 testnet.

Details of the NFT Token Contract can be viewed here - 
https://n3t4.neotube.io/contract/0x8b6a955ef8026cecaf9393f1734bffe508ce42be or 
https://testnet.explorer.onegate.space/contractinfo/0x8b6a955ef8026cecaf9393f1734bffe508ce42be

The NFTs can be minted by transferring GAS to the token contract which is facilitated by https://client.feedio.xyz

### Actual Contract Call

As we have seen so far the method getLatestTokenPrice has to be invoked from the Feedio contract 0xe0bd649469db432189f15cf9edbe5b1b8bd20a5f. 

The call is straightforward with only a single parameter required which is the token string for which the price needs to be fetched. The return type is a byte string which is the JSON encoded form of the price response.

```
ByteString latestPriceData = (ByteString) Contract.Call(ToScripthash("NUaWLwEL9bJZGmkpLesEmkkkmfDsFz1YGo"), "getLatestTokenPrice", CallFlags.All, (ByteString) "GAS");
```

The information once decoded will have the detail of the token (its name), the value in integers and the decimal places that has to be adjusted by the consumer/client. 

## Data Deserialization

After the above steps, here is the full call including the deserialization - 

```
        public static Map<string, object> GetTokenPrice() {
            
            ByteString latestPriceData = (ByteString) Contract.Call(ToScripthash("NUaWLwEL9bJZGmkpLesEmkkkmfDsFz1YGo"), "getLatestTokenPrice", CallFlags.All, (ByteString) "GAS");
            TokenPriceResponse priceResponse = (TokenPriceResponse)StdLib.JsonDeserialize(latestPriceData);
            Map<string, object> map = new Map<string, object>();
            map["name"] = priceResponse.name;
            map["value"] = priceResponse.value;
            map["decimals"] = priceResponse.decimals;

            return map;
        }
```

The TokenPriceResponse object encapsulates the data that is received from the Feedio contract. The structure of the class is as follows - 

```
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
```
