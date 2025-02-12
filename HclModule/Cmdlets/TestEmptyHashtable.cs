using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModPosh.Hclmodule.Cmdlets
{
    [Cmdlet(VerbsDiagnostic.Test, "EmptyHashtable")]
    public class TestEmptyHashtable : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public object Value { get; set; }

        protected override void ProcessRecord()
        {
            bool result = IsEmptyHashtable(Value);
            WriteObject(result);
        }

        private bool IsEmptyHashtable(object value)
        {
            if (value == null) return true;

            if (value is Hashtable hashtable && hashtable.Count == 0)
                return true;

            if (value is IDictionary dictionary && dictionary.Count == 0)
                return true;

            if (value is Hashtable ht && ht.ContainsKey("Default"))
            {
                var defaultValue = ht["Default"];
                if (defaultValue == null || (defaultValue is IDictionary dict && dict.Count == 0))
                    return true;
            }

            return false;
        }
    }
}
