using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModPosh.Hclmodule.Cmdlets
{
    [Cmdlet(VerbsCommon.Compare, "Variable")]
    public class CompareVariable : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public Hashtable Old { get; set; }

        [Parameter(Mandatory = true)]
        public Hashtable New { get; set; }

        protected override void ProcessRecord()
        {
            var result = CompareVariableSets(Old, New);
            WriteObject(result);
        }

        private Hashtable CompareVariableSets(Hashtable oldVars, Hashtable newVars)
        {
            var added = new List<string>();
            var removed = new List<string>();
            var modified = new List<Hashtable>();

            foreach (string key in newVars.Keys)
            {
                if (!oldVars.ContainsKey(key)) added.Add(key);
            }

            foreach (string key in oldVars.Keys)
            {
                if (!newVars.ContainsKey(key)) removed.Add(key);
            }

            foreach (string key in oldVars.Keys)
            {
                if (newVars.ContainsKey(key) && !oldVars[key].Equals(newVars[key]))
                {
                    modified.Add(new Hashtable
                    {
                        { "Variable", key },
                        { "OldValue", oldVars[key] },
                        { "NewValue", newVars[key] }
                    });
                }
            }

            return new Hashtable
            {
                { "Added", added },
                { "Removed", removed },
                { "Changed", modified }
            };
        }
    }
}
