#if UNITY_EDITOR
using Elympics;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CITools
{
	private const string ELYMPICS_USERNAME_ENV_VARIABLE = "ELYMPICS_USERNAME";
	private const string ELYMPICS_PASSWORD_ENV_VARIABLE = "ELYMPICS_PASSWORD";
	private const string ELYMPICS_ENDPOINT_ENV_VARIABLE = "ELYMPICS_ENDPOINT";
	private const string EXTRA_DEFINES_ENV_VARIABLE = "DEFINES";

	public static void BuildAndUploadServer()
	{
		var elympicsConfig = ElympicsConfig.Load();
		var gameIndex = 0;
		string version = GetGameVersion();
		SetGameVersion(elympicsConfig.AvailableGames[gameIndex], version);
		Debug.Log($"Building and uploading server for version >>> {version} <<<");
		var username = Environment.GetEnvironmentVariable(ELYMPICS_USERNAME_ENV_VARIABLE);
		var password = Environment.GetEnvironmentVariable(ELYMPICS_PASSWORD_ENV_VARIABLE);
		ElympicsWebIntegration.BuildAndUploadServerInBatchmode(username, password);
	}

	public static void BuildWindows()
	{
		Build(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone);
	}

	public static void BuildMacOS()
	{
		Build(BuildTarget.StandaloneOSX, BuildTargetGroup.Standalone);
	}

	public static void BuildWeb()
	{
		Build(BuildTarget.WebGL, BuildTargetGroup.WebGL);
	}

	private static void Build(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup)
	{
		string definesString = Environment.GetEnvironmentVariable(EXTRA_DEFINES_ENV_VARIABLE) ?? string.Empty;
		var defines = definesString
			.Split(';')
			.Where(s => !string.IsNullOrEmpty(s));

		var elympicsConfig = ElympicsConfig.Load();
		string version = GetGameVersion();
		foreach (var game in elympicsConfig.AvailableGames)
			SetGameVersion(game, version);

		Debug.Log($"Building {buildTarget} client");
		Debug.Log($"Scripting defines: {string.Join(" ", defines)}");
		BuildPipeline.BuildPlayer(GetBuildOptions(buildTarget, buildTargetGroup, defines.ToArray()));
	}


	private static void SetGameVersion(ElympicsGameConfig game, string version)
	{
		game.GameVersion = version;
		EditorUtility.SetDirty(game);
		AssetDatabase.SaveAssetIfDirty(game);
	}

	private static BuildPlayerOptions GetBuildOptions(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup, string[] defines)
	{
		return new BuildPlayerOptions()
		{
			scenes = GetScenesForBuild(),
			locationPathName = GetBuildLocation(),
			target = buildTarget,
			targetGroup = buildTargetGroup,
			extraScriptingDefines = defines
		};
	}

	private static string[] GetScenesForBuild()
	{
		return EditorBuildSettings.scenes
			.Where(scene => scene.enabled)
			.Select(scene => scene.path)
			.Where(path => !string.IsNullOrEmpty(path))
			.ToArray();
	}

	private static string GetBuildLocation() => GetArgument(2);

	private static string GetGameVersion() => GetArgument(1);

	private static string GetArgument(int number)
	{
		var arguments = Environment.GetCommandLineArgs();
		return arguments[arguments.Length - number];
	}
}
#endif

