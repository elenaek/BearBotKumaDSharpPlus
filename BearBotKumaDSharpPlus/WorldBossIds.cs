using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearBotKumaDSharpPlus
{
    public class WorldBossIds
    {

        //Pseudo enumerated boss name/guid mappings
        public const String shatterer = "03BF176A-D59F-49CA-A311-39FC6F533F2F";
        public const String svanirshaman = "F7D9D427-5E54-4F12-977A-9809B23FBA99";
        public const String modniirulgoth = "E6872A86-E434-4FC1-B803-89921FF0F6D6";
        public const String fireelemental = "33F76E9E-0BB6-46D0-A3A9-BE4CDFC4A3A4";
        public const String karkaqueen = "E1CC6E63-EFFE-4986-A321-95C89EA58C07";
        public const String junglewurm = "C5972F64-B894-45B4-BC31-2DEEA6B7C033";
        public const String golemmarkii = "9AA133DC-F630-4A0E-BB5D-EE34A2B306C2";
        public const String shadowbehemoth = "31CEBA08-E44D-472F-81B0-7143D73797F5";
        public const String tequatl = "568A30CF-8512-462F-9D67-647D69BEFAED";
        public const String clawofjormag = "0464CB9E-1848-4AAA-BA31-4779A959DD71";
        public const String tripletrouble = "F06787F5-CFC0-499A-954E-EC125FA71C9D";
        public const String taidhacovington = "242BD241-E360-48F1-A8D9-57180E146789";
        public const String megadestroyer = "C876757A-EF3E-4FBE-A484-07FF790D9B05";

        //Pseudo enumerated bossWPs
        public const String shattererWP = "[&BE4DAAA=]";
        public const String svanirshamanWP = "[&BMIDAAA=]";
        public const String modniirulgothWP = "[&BLAAAAA=]";
        public const String fireelementalWP = "[&BEcAAAA=]";
        public const String karkaqueenWP = "[&BNUGAAA=]";
        public const String junglewurmWP = "[&BEEFAAA=]";
        public const String golemmarkiiWP = "[&BNQCAAA=]";
        public const String shadowbehemothWP = "[&BPcAAAA=]";
        public const String tequatlWP = "[&BNABAAA=]";
        public const String clawofjormagWP = "[&BHoCAAA=]";
        public const String tripletroubleWP = "[&BKoBAAA=]";
        public const String taidhacovingtonWP = "[&BKgBAAA=]";
        public const String megadestroyerWP = "[&BM0CAAA=]";



        public static WorldBossInfo GetBossInfo(string guid)
        {
            switch (guid.ToUpper())
            {
                case shatterer:
                    return new WorldBossInfo(guid, "Shatterer",shattererWP);
                case svanirshaman:
                    return new WorldBossInfo(guid, "Svanir Shaman", svanirshamanWP);
                case modniirulgoth:
                    return new WorldBossInfo(guid, "Modniir Ulgoth", modniirulgothWP);
                case fireelemental:
                    return new WorldBossInfo(guid, "Fire Elemental", fireelementalWP);
                case karkaqueen:
                    return new WorldBossInfo(guid, "Karka Queen", karkaqueenWP);
                case junglewurm:
                    return new WorldBossInfo(guid, "Jungle Wurm", junglewurmWP);
                case golemmarkii:
                    return new WorldBossInfo(guid, "Golem Mark II", golemmarkiiWP);
                case shadowbehemoth:
                    return new WorldBossInfo(guid, "Shadow Behemoth", shadowbehemothWP);
                case tequatl:
                    return new WorldBossInfo(guid, "Tacoquatl", tequatlWP);
                case clawofjormag:
                    return new WorldBossInfo(guid, "Claw of Jormag", clawofjormagWP);
                case tripletrouble:
                    return new WorldBossInfo(guid, "Triple Trouble", tripletroubleWP);
                case taidhacovington:
                    return new WorldBossInfo(guid, "Admiral Taidha", taidhacovingtonWP);
                case megadestroyer:
                    return new WorldBossInfo(guid, "Megadestroyer", megadestroyerWP);
                    
            }

            return null;
        }

    }

    
}
