using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PerformBuildStep
{
  static void PerformBuild ()
  {
    Console.WriteLine("Start building");
	var buildTarget = GetBuildTarget();
	var buildPath = GetBuildPath();
	var buildName = GetBuildName();
	var fixedBuildPath = GetFixedBuildPath(buildTarget, buildPath, buildName);
        
    Console.WriteLine("Fixed build path:" + fixedBuildPath);
    var buildInfo = BuildPipeline.BuildPlayer(GetEnabledScenes(), fixedBuildPath, buildTarget, GetBuildOptions());
	if(buildInfo.summary.result == BuildResult.Succeeded) 
    {
      Console.WriteLine("Build Completed!");
	}
	else 
    {
	  Console.WriteLine("BUILD ERROR");
      foreach(var step in buildInfo.steps) 
      {
        foreach(var message in step.messages) 
		  Console.WriteLine(step.name + " -- " + message.content);
      }
      EditorApplication.Exit(1);
	}
  }
}
