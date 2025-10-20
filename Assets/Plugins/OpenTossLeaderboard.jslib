mergeInto(LibraryManager.library, {
  OpenTossLeaderboard: function() {
    console.log("Unity → JSlib → openTossLeaderboard 호출됨!");
    if (typeof window !== 'undefined' && window.openTossLeaderboard) {
      window.openTossLeaderboard();
    } else {
      console.warn("window.openTossLeaderboard is not defined");
    }
  },
  CheckTossAppVersion: function() {
    console.log("Unity → JSlib → checkTossAppVersion 호출됨!");
    if (typeof window !== 'undefined' && window.checkTossAppVersion) {
      window.checkTossAppVersion();
    } else {
      console.warn("window.checkTossAppVersion is not defined");
    }
  }
});