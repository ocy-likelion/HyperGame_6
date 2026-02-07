mergeInto(LibraryManager.library, {
  GetSafeAreaInsets: function() {
    console.log("Unity → JSlib → getSafeAreaInsets 호출됨!");
    if (typeof window !== 'undefined' && window.getSafeAreaInsets) {
      window.getSafeAreaInsets();
    } else {
      console.warn("window.getSafeAreaInsets is not defined");
    }
  }
});