//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FeedioSampleTests {
    #if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
    [System.CodeDom.Compiler.GeneratedCode("Neo.BuildTasks","1.0.40.7585")]
    #endif
    [System.ComponentModel.Description("FeedioSampleContract")]
    interface FeedioSampleContract {
        Neo.VM.Types.Map getTokenPrice();
        void onNEP11Payment(Neo.UInt160 @from, Neo.UInt160 to, byte[] tokenId, object @data);
        void updateContract(byte[] nefFile, string manifest);
        void destroy();
    }
}
