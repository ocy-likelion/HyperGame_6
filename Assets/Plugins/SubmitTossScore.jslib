mergeInto(LibraryManager.library, {
  SubmitTossScore: function(score) {
    try {
      // 동기 예외가 Unity로 전파되지 않게 프레임 분리
      setTimeout(function () {
        try {
          if (typeof window !== 'undefined' && window.submitTossScore) {
            window.submitTossScore(score);
          } else {
            console.warn("window.submitTossScore not defined");
          }
        } catch (e) {
          console.error("submitTossScore inner error:", e);
        }
      }, 0);
    } catch (e) {
      console.error("SubmitTossScore outer error:", e);
    }
  }
});