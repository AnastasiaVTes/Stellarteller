using System.Collections;
using UnityEngine;
using RTS;
	
	namespace RTS {
    public static class ResourceManager{
        //for class governing camera movement/rotation - srsly I don't need this shit - maybe delete later
        public static float ScrollSpeed { get { return 25; } }
        public static float RotateSpeed { get { return 100; } }
        public static int ScrollWidth { get { return 15; } }
        public static float MinCameraHeight { get { return 10; } }
        public static float MaxCameraHeight { get { return 40; } }
        public static float RotateAmount { get { return 10; } }
        //end
        //for modified Fly_Cam 
        private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
        public static Vector3 InvalidPosition { get { return invalidPosition; } }
        //end of Fly_Cam for object selection in part 5
        //drawing in selection box worldobject ongui
        private static GUISkin selectBoxSkin;
        public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }

        public static void StoreSelectBoxItems(GUISkin skin)
        {
            selectBoxSkin = skin;
        }
        //end
        //bounds
        private static Bounds invalidBounds = new Bounds(new Vector3(-99999, -99999, -99999), new Vector3(0, 0, 0));
        public static Bounds InvalidBounds { get { return invalidBounds; } }
        //end
        //building units
        public static int BuildSpeed { get { return 2; } }
        //end
        //building stuff in gameobjectlist
        //public static void SetGameObjectList(GameObjectList objectList)
        //{
        //    gameObjectList = objectList;
        //}
        //private static GameObjectList gameObjectList;

        //public static GameObject GetBuilding(string name)
        //{
        //    return gameObjectList.GetBuilding(name);
        //}

        //public static GameObject GetUnit(string name)
        //{
        //    return gameObjectList.GetUnit(name);
        //}

        //public static GameObject GetWorldObject(string name)
        //{
        //    return gameObjectList.GetWorldObject(name);
        //}

        //public static GameObject GetPlayerObject()
        //{
        //    return gameObjectList.GetPlayerObject();
        //}

        //public static Texture2D GetBuildImage(string name)
        //{
        //    return gameObjectList.GetBuildImage(name);
        //}
        //end

    }
    }