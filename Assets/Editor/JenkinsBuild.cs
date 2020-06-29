using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System;

class JenkinsBuild
{
    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "Test_CI";
    static string TARGET_DIR = "Builds";

    [MenuItem("Custom/CI/Build Mac OS X")]
    static void PerformMacOSXBuild()
    {
        string targetPath = APP_NAME + ".app";
        GenericBuild(SCENES, TARGET_DIR + "/" + targetPath, BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build Windows")]
    static void PerformWindowsBuild()
    {
        string targetPath = APP_NAME + ".exe";
        GenericBuild(SCENES, TARGET_DIR + "/" + targetPath, BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build Android")]
    static void PerformAndroidBuild()
    {
        string targetPath = APP_NAME + ".apk";
        GenericBuild(SCENES, TARGET_DIR + "/" + targetPath, BuildTargetGroup.Standalone, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string targetDir, BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = targetDir;
        buildPlayerOptions.targetGroup = buildTargetGroup;
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = buildOptions;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (report.summary.totalErrors != 0)
        {
            throw new Exception("BuildPlayer failure: " + report.summary.totalErrors + "(errors)");
        }
    }
}