﻿using System;
using Handbook;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

public class VideoFix:FortressCraftMod
{
    private bool replacedVideo;
    private bool replacedWelcome;
    private static readonly string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static readonly string videoXmlPath = Path.Combine(assemblyFolder, "VideoTutorials.xml");
    private static readonly string welcomeXmlPath = Path.Combine(assemblyFolder, "Welcome.xml");

    public override ModRegistrationData Register()
    {
        ModRegistrationData modRegistrationData = new ModRegistrationData();
        return modRegistrationData;
    }

    public void Update()
    {
        if (HandbookXML.mSurvivalChaptersByKey != null)
        {
            if (HandbookXML.mSurvivalChaptersByKey.ContainsKey("VideoTutorials") && !replacedVideo)
            {
                HandbookXML.mSurvivalChaptersByKey["VideoTutorials"] = LoadChapter(videoXmlPath);
                HandbookXML.mSurvivalChapters = new List<Chapter>(HandbookXML.mSurvivalChaptersByKey.Values);
                replacedVideo = true;
            }

            if (HandbookXML.mSurvivalChaptersByKey.ContainsKey("welcome") && !replacedWelcome)
            {
                HandbookXML.mSurvivalChaptersByKey["welcome"] = LoadChapter(welcomeXmlPath);
                HandbookXML.mSurvivalChapters = new List<Chapter>(HandbookXML.mSurvivalChaptersByKey.Values);
                replacedWelcome = true;
            }
        }
    }

    private static Chapter LoadChapter(string path)
    {
        if (path == null || !File.Exists(path))
        {
            return null;
        }
        try
        {
            Chapter chapter = (Chapter)XMLParser.ReadXML(path, typeof(Chapter));
            if (chapter.Icon == null)
            {
                chapter.Icon = chapter.Title;
            }
            return chapter;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Failed to read handbook chapter: " + path + "\nReason: " + ex.Message);
            return null;
        }
    }
}