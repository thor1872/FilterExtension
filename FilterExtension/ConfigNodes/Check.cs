﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FilterExtensions.ConfigNodes
{
    using Utility;

    public class Check
    {
        public string type { get; set; } // type of check to perform (module, title/name, resource,...)
        public string value { get; set; }
        public bool invert { get; set; }
        public bool contains { get; set; }
        public List<Check> checks { get; set; } 

        internal Check(ConfigNode node)
        {
            type = node.GetValue("type");
            value = node.GetValue("value");

            bool tmp;
            bool.TryParse(node.GetValue("invert"), out tmp);
            invert = tmp;

            bool success = bool.TryParse(node.GetValue("contains"), out tmp);
            if (success)
                contains = tmp;
            else
                contains = true;

            //if (type == "check")
            //{
            //    checks = new List<Check>();
            //    foreach (ConfigNode subNode in node.GetNodes("CHECK"))
            //    {
            //        checks.Add(new Check(subNode));
            //    }
            //}
        }

        internal Check(string type, string value, bool invert = false, bool contains = true)
        {
            this.type = type;
            this.value = value;
            this.invert = invert;
            this.contains = contains;
        }

        internal bool checkPart(AvailablePart part)
        {
            bool result = true;

            if (result)
            {
                switch (type)
                {
                    case "moduleTitle": // check by module title
                        result = PartType.checkModuleTitle(part, value, contains);
                        break;
                    case "moduleName":
                        result = PartType.checkModuleName(part, value, contains);
                        break;
                    case "name": // check by part name (cfg name)
                        result = PartType.checkName(part, value);
                        break;
                    case "title": // check by part title (in game name)
                        result = PartType.checkTitle(part, value);
                        break;
                    case "resource": // check for a resource
                        result = PartType.checkResource(part, value, contains);
                        break;
                    case "propellant": // check for engine propellant
                        result = PartType.checkPropellant(part, value, contains);
                        break;
                    case "tech": // check by tech
                        result = PartType.checkTech(part, value);
                        break;
                    case "manufacturer": // check by manufacturer
                        result = PartType.checkManufacturer(part, value);
                        break;
                    case "folder": // check by mod root folder
                        result = PartType.checkFolder(part, value);
                        break;
                    case "category":
                        result = PartType.checkCategory(part, value);
                        break;
                    case "size": // check by largest stack node size
                        result = PartType.checkPartSize(part, value, contains);
                        break;
                    case "crew":
                        result = PartType.checkCrewCapacity(part, value);
                        break;
                    case "custom": // for when things get tricky
                        result = PartType.checkCustom(part, value);
                        break;
                    case "check":

                        break;
                    default:
                        result = false;
                        break;
                }
            }
            
            if (invert)
                result = !result;

            return result;
        }

        public bool Equals(Check c2)
        {
            if (c2 == null)
                return false;
            if (this.type == c2.type && this.value == c2.value && this.invert == c2.invert)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.type.GetHashCode() * this.value.GetHashCode() * this.invert.GetHashCode();
        }
    }
}
