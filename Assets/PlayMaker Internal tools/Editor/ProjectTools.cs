using System;
using System.Collections;

using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using HutongGames.PlayMakerEditorUtils;

namespace HutongGames.PlayMakerEditor
{
	public class ProjectToolsTest
	{
		// Change MenuRoot to move the Playmaker Menu
		// E.g., MenuRoot = "Plugins/PlayMaker/"
		private const string MenuRoot = "PlayMaker Test/";
		
		[MenuItem(MenuRoot + "Tools/Re-Save All Loaded FSMs", false, 31)]
		public static void ReSaveAllLoadedFSMs()
		{
			EditorCoroutine.start(DoReSaveAllLoadedFSMs());
		}

		/*
		static IEnumerator DoSerializeAllScenes()
		{
			AbortSerializingProcessFlag = false;
			SerializingProcessActive = true;
			LoadLevelContentXml();
			yield return null;
			EditorCoroutine f1e = EditorCoroutine.startManual(PerformSerializeAllScenes());
			while (f1e.routine.MoveNext()) {
				yield return f1e.routine.Current;
			}
			yield return null ;
			SaveLevelContentxml();
			_chapterIdFilter = null;
			SerializingProcessActive = false;
			
		}
	*/
		static IEnumerator DoReSaveAllLoadedFSMs()
		{
			Debug.Log("------ Start DoReSaveAllLoadedFSMs --------- ");

			Debug.Log("-- start LoadPrefabsWithPlayMakerFSMComponents ");

			LoadPrefabsWithPlayMakerFSMComponents();
			yield return null;

			Debug.Log("-- start SaveAllLoadedFSMs ");
			SaveAllLoadedFSMs();

			yield return null;

			Debug.Log("-- start SaveAllTemplates ");

			SaveAllTemplates();

			yield return null;

			Debug.Log("------ End DoReSaveAllLoadedFSMs --------- ");

		}


		[MenuItem(MenuRoot + "Tools/Re-Save All FSMs in Build", false, 32)]
		public static void ReSaveAllFSMsInBuild()
		{
			EditorCoroutine.start(DoReSaveAllFSMsInBuild());
		}

		static IEnumerator DoReSaveAllFSMsInBuild()
		{
			Debug.Log("------ Start DoReSaveAllFSMsInBuild --------- ");

			Debug.Log("-- start SaveAllLoadedFSMs ");

			LoadPrefabsWithPlayMakerFSMComponents();
			yield return null;
			
			Debug.Log("-- start SaveAllLoadedFSMs ");
			SaveAllFSMsInBuild();
			
			yield return null;
			
			Debug.Log("-- start SaveAllTemplates ");
			
			SaveAllTemplates();
			
			yield return null;
			
			Debug.Log("------ End DoReSaveAllLoadedFSMs --------- ");
			
		}



		[MenuItem(MenuRoot + "Tools/Scan Scenes", false, 33)]
		public static void ScanScenesInProject()
		{
			FindAllScenes();
		}
		
		private static void SaveAllTemplates()
		{
			Debug.Log("Re-Saving All Templates...");
			
			FsmEditorUtility.BuildTemplateList();
			foreach (var template in FsmEditorUtility.TemplateList)
			{
				FsmEditor.SetFsmDirty(template.fsm, false);
				Debug.Log("Re-save Template: " + template.name);
			}
		}
		
		private static void SaveAllLoadedFSMs()
		{
			FsmEditor.RebuildFsmList();
			foreach (var fsm in FsmEditor.FsmList)
			{
				Debug.Log("Re-save FSM: " + FsmEditorUtility.GetFullFsmLabel(fsm));
				//FsmEditor.SetFsmDirty(fsm, false);
				FsmEditor.SaveActions(fsm);
			}
		}
		
		private static void SaveAllFSMsInBuild()
		{
			foreach (var scene in EditorBuildSettings.scenes)
			{
				Debug.Log("Open Scene: " + scene.path);
				EditorApplication.OpenScene(scene.path);
				FsmEditor.RebuildFsmList();
				SaveAllLoadedFSMs();
				EditorApplication.SaveScene();
			}
		}
		
		private static void LoadPrefabsWithPlayMakerFSMComponents()
		{
			Debug.Log("Finding Prefabs with PlayMakerFSMs");
			
			var searchDirectory = new DirectoryInfo(Application.dataPath);
			var prefabFiles = searchDirectory.GetFiles("*.prefab", SearchOption.AllDirectories);
			
			foreach (var file in prefabFiles)
			{
				var filePath = file.FullName.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
				//Debug.Log(filePath + "\n" + Application.dataPath);
				
				var dependencies = AssetDatabase.GetDependencies(new[] { filePath });
				foreach (var dependency in dependencies)
				{
					if (dependency.Contains("/PlayMaker.dll"))
					{
						Debug.Log("Found Prefab with FSM: " + filePath);
						AssetDatabase.LoadAssetAtPath(filePath, typeof(GameObject));
					}
				}
			}
			
			FsmEditor.RebuildFsmList();
		}
		
		
		[Localizable(false)]
		private static void FindAllScenes()
		{
			Debug.Log("Finding all scenes...");
			
			var searchDirectory = new DirectoryInfo(Application.dataPath);
			var assetFiles = searchDirectory.GetFiles("*.unity", SearchOption.AllDirectories);
			
			foreach (var file in assetFiles)
			{
				var filePath = file.FullName.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
				var obj = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
				if (obj == null)
				{
					//Debug.Log(filePath + ": null!");
				}
				else if (obj.GetType() == typeof(Object))
				{
					Debug.Log(filePath);// + ": " + obj.GetType().FullName);
				}
				//var obj = AssetDatabase.
			}
		}
		
	}
	
	
}

