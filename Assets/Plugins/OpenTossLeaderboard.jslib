mergeInto(LibraryManager.library, {
  OpenTossLeaderboard: function() {
    console.log("Unity → JSlib → openTossLeaderboard 호출됨!");
    if (typeof window !== 'undefined' && window.openTossLeaderboard) {
      window.openTossLeaderboard();
    } else {
      console.warn("window.openTossLeaderboard is not defined");
    }
  }
});