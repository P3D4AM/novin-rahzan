
StiJsViewer.prototype.InitializeEvents = function () {
    var jsObject = this;
    var viewer = this.controls.viewer;

    if (viewer && viewer.offsetWidth && viewer.offsetHeight) {
        jsObject.service.widthBeforeResize = viewer.offsetWidth;
        jsObject.service.heightBeforeResize = viewer.offsetHeight;
    }

    var resizeEvent = function (e) {
        if (jsObject.reportParams.type == "Dashboard") {
            if (jsObject.service.resizeTimer) clearTimeout(jsObject.service.resizeTimer);
            jsObject.service.resizeTimer = setTimeout(function () {
                if (jsObject.options.jsDesigner && !jsObject.options.jsDesigner.options.previewMode ||
                    !viewer || !viewer.offsetWidth || !viewer.offsetHeight) return;

                if (jsObject.service.widthBeforeResize != viewer.offsetWidth || jsObject.service.heightBeforeResize != viewer.offsetHeight) {
                    jsObject.service.widthBeforeResize = viewer.offsetWidth;
                    jsObject.service.heightBeforeResize = viewer.offsetHeight;
                    jsObject.postAction("GetPages");
                }
            }, 300);
        }
    }

    this.addEvent(window, "resize", resizeEvent);
}