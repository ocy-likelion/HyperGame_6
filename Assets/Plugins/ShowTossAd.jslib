mergeInto(LibraryManager.library, {
  LoadInterstitialAd: function() {
    setTimeout(() => {
      if (window.loadAdMobInterstitialAd) window.loadAdMobInterstitialAd();
      else console.warn("window.loadAdMobInterstitialAd not defined");
    }, 0);
  },
  ShowInterstitialAd: function() {
    setTimeout(() => {
      if (window.showAdMobInterstitialAd) window.showAdMobInterstitialAd();
      else console.warn("window.showAdMobInterstitialAd not defined");
    }, 0);
  }
});