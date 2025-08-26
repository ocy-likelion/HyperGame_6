using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    public async Task<Sprite> LoadSpriteData(string address)
    {
        var asset = await DataLoader.DataLoad(address);
        return asset;
    }
    
    public async Task<Sprite[]> LoadSpritesData(string address)
    {
        var asset = await DataLoader.DataArrayLoad(address);
        return asset;
    }

    public void ClearData()
    {
        DataLoader.ClearCache();
    }
    
    public static class DataLoader
    {
        private static readonly Dictionary<string, Sprite> _caches = new();
        private static readonly Dictionary<string, Sprite[]> _arraycaches = new();
        public static async Task<Sprite> DataLoad(string address)
        {
            if (_caches.TryGetValue(address, out var cached))
            {
                return cached;
            }
        
            var asset =  await Addressables.LoadAssetAsync<Sprite>(address).Task;
            _caches[address] = asset;
        
            return asset;
        }
        
        public static async Task<Sprite[]> DataArrayLoad(string address)
        {
            if (_arraycaches.TryGetValue(address, out var _arraycached))
            {
                return _arraycached;
            }

            var asset = await Addressables.LoadAssetAsync<Sprite[]>(address).Task;
            var newAsset = asset.OrderBy(s => s.name).ToArray();
            
            _arraycaches[address] = asset;
        
            return newAsset;
        }
        
        public static void ClearCache()
        {
            foreach (var asset in _caches.Values)
            {
                if(asset != null) Addressables.Release(asset);
            }
            _caches?.Clear();

            foreach (var asset in _arraycaches.Values)
            {
                if(asset != null) Addressables.Release(asset);
            }
            _arraycaches?.Clear();
        }
    }

    public new void OnDestroy()
    {
        ClearData();
    }
}