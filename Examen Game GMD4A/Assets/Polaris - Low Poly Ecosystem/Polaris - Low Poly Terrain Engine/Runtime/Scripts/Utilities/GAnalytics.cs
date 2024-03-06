using UnityEngine;
using UnityEngine.Networking;

namespace Pinwheel.Griffin
{
    public static class GAnalytics
    {
        public const string OS_WINDOWS = "https://bit.ly/34fMuhb";
        public const string OS_MAC = "https://bit.ly/31XORUx";
        public const string OS_LINUX = "https://bit.ly/34cUYWg";

        public const string PLATFORM_PC = "https://bit.ly/333nKZq";
        public const string PLATFORM_MOBILE = "https://bit.ly/349N0gA";
        public const string PLATFORM_CONSOLE = "https://bit.ly/2NrXvVN";
        public const string PLATFORM_WEB = "https://bit.ly/36dOsQZ";
        public const string PLATFORM_OTHER = "https://bit.ly/2MZnuFm";

        public const string XR_PROJECT = "https://bit.ly/2qYUhlr";

        public const string COLOR_SPACE_GAMMA = "https://bit.ly/330OHwG";
        public const string COLOR_SPACE_LINEAR = "https://bit.ly/349O7wM";

        public const string RENDER_PIPELINE_BUILTIN = "https://bit.ly/2N091Jg";
        public const string RENDER_PIPELINE_LIGHTWEIGHT = "https://bit.ly/36hxUro";
        public const string RENDER_PIPELINE_UNIVERSAL = "https://bit.ly/34fbaX7";
        public const string RENDER_PIPELINE_OTHER = "https://bit.ly/2piMkaf";

        public const string INTEGRATION_AMPLIFY_SHADER_EDITOR = "https://bit.ly/2JBwfDw";
        public const string INTEGRATION_POSEIDON = "https://bit.ly/2WrPEMc";
        public const string INTEGRATION_CSHARP_WIZARD = "https://bit.ly/2MZsqdh";
        public const string INTEGRATION_MESH_TO_FILE = "https://bit.ly/2Nr35b1";
        public const string INTEGRATION_VEGETATION_STUDIO = "https://bit.ly/34kiPVB";
        public const string INTEGRATION_MICRO_SPLAT = "https://bit.ly/3bwXtWS";

        public const string MULTI_TERRAIN = "https://bit.ly/2q2M2Eh";
        public const string WIND_ZONE = "https://bit.ly/2JCtjWY";
        public const string CONVERT_FROM_UNITY_TERRAIN = "https://bit.ly/2WrphpN";

        public const string WIZARD_CREATE_TERRAIN = "https://bit.ly/2PurvTT";
        public const string WIZARD_SET_SHADER = "https://bit.ly/326NG5j";

        public const string SHADING_COLOR_MAP = "https://bit.ly/323pzEg";
        public const string SHADING_GRADIENT_LOOKUP = "https://bit.ly/2PB1WR0";
        public const string SHADING_SPLAT = "https://bit.ly/2C0jmi1";
        public const string SHADING_VERTEX_COLOR = "https://bit.ly/2q6Ty13";

        public const string ENABLE_INSTANCING = "https://bit.ly/2PAjM6C";
        public const string ENABLE_INTERACTIVE_GRASS = "https://bit.ly/2BWRMSD";

        public const string IMPORT_UNITY_TERRAIN_DATA = "https://bit.ly/2JApdPl";
        public const string IMPORT_POLARIS_V1_DATA = "https://bit.ly/34bKdDI";
        public const string IMPORT_RAW = "https://bit.ly/2qZK5sO";
        public const string IMPORT_TEXTURES = "https://bit.ly/2NqHYFY";

        public const string EXPORT_UNITY_TERRAIN_DATA = "https://bit.ly/2N34cyT";
        public const string EXPORT_RAW = "https://bit.ly/2Ws3XAg";
        public const string EXPORT_TEXTURES = "https://bit.ly/335KGak";

        public const string GROUP_OVERRIDE_GEOMETRY = "https://bit.ly/2N4ho6G";
        public const string GROUP_OVERRIDE_SHADING = "https://bit.ly/31VkuOs";
        public const string GROUP_OVERRIDE_RENDERING = "https://bit.ly/2NpGf3C";
        public const string GROUP_OVERRIDE_FOLIAGE = "https://bit.ly/2qZMhR4";
        public const string GROUP_REARRANGE = "https://bit.ly/2JDdW0t";
        public const string GROUP_MATCH_EDGE = "https://bit.ly/2rDM46g";

        public const string TPAINTER_ELEVATION = "https://bit.ly/2pg6dPe";
        public const string TPAINTER_HEIGHT_SAMPLING = "https://bit.ly/36ihqiH";
        public const string TPAINTER_TERRACE = "https://bit.ly/32XeeqE";
        public const string TPAINTER_REMAP = "https://bit.ly/34fXZ8t";
        public const string TPAINTER_NOISE = "https://bit.ly/2ptazlQ";
        public const string TPAINTER_SUBDIV = "https://bit.ly/2qPboWu";
        public const string TPAINTER_ALBEDO = "https://bit.ly/2otnoMz";
        public const string TPAINTER_METALLIC = "https://bit.ly/2JwqyGG";
        public const string TPAINTER_SMOOTHNESS = "https://bit.ly/2NpLkJm";
        public const string TPAINTER_SPLAT = "https://bit.ly/36aCDLo";
        public const string TPAINTER_CUSTOM = "https://bit.ly/33bdH4o";

        public const string FPAINTER_PAINT_TREE = "https://bit.ly/36dGwzb";
        public const string FPAINTER_SCALE_TREE = "https://bit.ly/2JzHJHt";
        public const string FPAINTER_PAINT_GRASS = "https://bit.ly/2MWmfqm";
        public const string FPAINTER_SCALE_GRASS = "https://bit.ly/2Pv2EiH";
        public const string FPAINTER_CUSTOM = "https://bit.ly/34dvJ6f";

        public const string OPAINTER_SPAWN = "https://bit.ly/36mPn1R";
        public const string OPAINTER_SCALE = "https://bit.ly/2BRbHCC";
        public const string OPAINTER_CUSTOM = "https://bit.ly/2PtNhHi";

        public const string SPLINE_RAMP_MAKER = "https://bit.ly/3337V50";
        public const string SPLINE_PATH_PAINTER = "https://bit.ly/2NsN7gD";
        public const string SPLINE_FOLIAGE_SPAWNER = "https://bit.ly/3307hW0";
        public const string SPLINE_FOLIAGE_REMOVER = "https://bit.ly/2WqoeGC";
        public const string SPLINE_OBJECT_SPAWNER = "https://bit.ly/2qbnFEg";
        public const string SPLINE_OBJECT_REMOVER = "https://bit.ly/2BVVxI4";

        public const string STAMPER_GEOMETRY = "https://bit.ly/2q5nOJy";
        public const string STAMPER_TEXTURE = "https://bit.ly/2JDEU8a";
        public const string STAMPER_FOLIAGE = "https://bit.ly/321JIe3";
        public const string STAMPER_OBJECT = "https://bit.ly/34ia3WC";

        public const string NAVIGATION_HELPER = "https://bit.ly/2NqLwrM";

        public const string BACKUP_CREATE = "https://bit.ly/2N2NzDf";
        public const string BACKUP_RESTORE = "https://bit.ly/2r20Ofb";

        public const string ASSET_EXPLORER_LINK_CLICK = "https://bit.ly/34iwLhr";
        public const string HELP_OPEN_WINDOW = "https://bit.ly/2pv2i0N";
        public const string BILLBOARD_SAVE = "https://bit.ly/333aaVY";

        public const string TEXTURE_CREATOR_HEIGHT_MAP = "https://bit.ly/2WqqWvM";
        public const string TEXTURE_CREATOR_HEIGHT_MAP_FROM_MESH = "https://bit.ly/2pv2YmR";
        public const string TEXTURE_CREATOR_NORMAL_MAP = "https://bit.ly/2WrJdIW";
        public const string TEXTURE_CREATOR_STEEPNESS_MAP = "https://bit.ly/2Py71cT";
        public const string TEXTURE_CREATOR_NOISE_MAP = "https://bit.ly/2JzBtQ8";
        public const string TEXTURE_CREATOR_COLOR_MAP = "https://bit.ly/2N37emP";
        public const string TEXTURE_CREATOR_BLEND_MAP = "https://bit.ly/2Ws8H92";
        public const string TEXTURE_CREATOR_FOLIAGE_DISTRIBUTION_MAP = "https://bit.ly/322zCJU";

        public const string LINK_ONLINE_MANUAL = "https://bit.ly/2NvamGK";
        public const string LINK_YOUTUBE = "https://bit.ly/2N0s2uU";
        public const string LINK_FACEBOOK = "https://bit.ly/2pjN278";
        public const string LINK_EXPLORE_ASSET = "https://bit.ly/2PFqDvs";

        public static void Record(string url, bool perProject = false)
        {
#if UNITY_EDITOR
            if (!GEditorSettings.Instance.general.enableAnalytics)
                return;

            if (string.IsNullOrEmpty(url))
                return;

            bool willRecord = true;
            if (perProject && PlayerPrefs.HasKey(url))
            {
                willRecord = false;
            }

            if (!willRecord)
                return;

            if (perProject)
            {
                PlayerPrefs.SetInt(url, 1);
            }

            UnityWebRequest request = new UnityWebRequest(url);
            request.SendWebRequest();
#endif
        }
    }
}
