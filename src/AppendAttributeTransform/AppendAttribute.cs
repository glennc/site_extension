using Microsoft.Web.XmlTransform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppendTransforms
{
    public class InsertOrAppendAttribute : Transform
    {
        public InsertOrAppendAttribute()
            : base(TransformFlags.UseParentAsTargetNode, MissingTargetMessage.Error)
        {
        }

        private string attributeName;

        protected string AttributeName
        {
            get
            {
                if (this.attributeName == null)
                {
                    this.attributeName = this.GetArgumentValue("Attribute");
                }
                return this.attributeName;
            }
        }
        protected string GetArgumentValue(string name)
        {
            // this extracts a value from the arguments provided
            if (string.IsNullOrWhiteSpace(name))
            { throw new ArgumentNullException("name"); }

            string result = null;
            if (this.Arguments != null && this.Arguments.Count > 0)
            {
                foreach (string arg in this.Arguments)
                {
                    if (!string.IsNullOrWhiteSpace(arg))
                    {
                        string trimmedArg = arg.Trim();
                        if (trimmedArg.ToUpperInvariant().StartsWith(name.ToUpperInvariant()))
                        {
                            int start = arg.IndexOf('\'');
                            int last = arg.LastIndexOf('\'');
                            if (start <= 0 || last <= 0 || last <= 0)
                            {
                                throw new ArgumentException("Expected two ['] characters");
                            }

                            string value = trimmedArg.Substring(start, last - start);
                            if (value != null)
                            {
                                // remove any leading or trailing '
                                value = value.Trim().TrimStart('\'').TrimStart('\'');
                            }
                            result = value;
                        }
                    }
                }
            }
            return result;
        }

        protected override void Apply()
        {
            if (base.TargetChildNodes == null || base.TargetChildNodes.Count == 0)
            {
                base.TargetNode.AppendChild(base.TransformNode);
            }
            else
            {
                XmlAttribute transformAtt = null;

                foreach (XmlAttribute att in this.TransformNode.Attributes)
                {
                    if (string.Compare(att.Name, this.AttributeName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        transformAtt = att;
                        break;
                    }
                }

                if (transformAtt == null)
                {
                    throw new InvalidOperationException("No target attribute to append");
                }

                foreach (XmlNode targetNode in this.TargetChildNodes)
                {
                    foreach (XmlAttribute att in targetNode.Attributes)
                    {
                        if (string.Compare(att.Name, this.AttributeName, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            if (string.IsNullOrEmpty(att.Value))
                            {
                                att.Value = transformAtt.Value;
                            }
                            else
                            {
                                //Horrible hack becuase this doesn't compose super nice with
                                //insertOrAppend being applied on the TargetNode. The target node is created
                                //with the children it has in the transform, which means we would duplicate the value
                                //here.
                                if(att.Value == transformAtt.Value) { return; }
                                att.Value = $"{att.Value};{transformAtt.Value}";
                            }
                        }
                    }
                }
            }
        }
    }
}
