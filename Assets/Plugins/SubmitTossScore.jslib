mergeInto(LibraryManager.library, {
    SubmitTossScore: function(score) {
        console.log("Unity → JSlib SubmitTossScore:", score);
        if (typeof window !== 'undefined' && window.submitTossScore) {
            window.submitTossScore(score);
        } else {
            console.warn("window.submitTossScore not defined");
        }
    }
});