using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModPosh.Hclmodule.Cmdlets
{
    [Cmdlet(VerbsData.Convert, "Hcl")]
    public class ConvertHcl : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Content { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                // Assume Modposh.Hcl.Client is available
                var hclClient = new Modposh.Hcl.Client();
                var parsedData = hclClient.Parse(Content);

                var parsedVariables = new Hashtable();

                foreach (var variable in parsedData.Variables)
                {
                    var varName = variable.Name;
                    parsedVariables[varName] = new Hashtable
                    {
                        { "Type", variable.Body.ContainsKey("type") ? variable.Body["type"].Value : null },
                        { "Default", variable.Body.ContainsKey("default") ? variable.Body["default"].Value : null },
                        { "Description", variable.Body.ContainsKey("description") ? variable.Body["description"].Value : null }
                    };
                }

                WriteObject(parsedVariables);
            }
            catch (Exception ex)
            {
                WriteWarning("Error parsing HCL: " + ex.Message);
                WriteObject(new Hashtable());
            }
        }
    }
}
