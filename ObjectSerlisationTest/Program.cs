// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using DroneManager.Interface;
using TestAssetImpl;
using TestAssetImpl = TestAssetImpl.TestAssetImpl;

Console.WriteLine("Hello, World!");

var asset = new global::TestAssetImpl.TestAssetImpl();

AssetManager.AssetManager.Instance.FileManager.SaveAsset(asset, asset.Drone.Id.ToString());

var loaded = AssetManager.AssetManager.Instance.FileManager.LoadAsset<global::TestAssetImpl.TestAssetImpl>
    (asset.Drone.Id.ToString());
Debugger.Break();