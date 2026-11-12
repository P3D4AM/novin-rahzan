
StiJsViewer.prototype.AddInteractionsToIndicatorElement = function (element, mainContainer) {
    var jsObject = this;
    var elementAttrs = element.elementAttributes;
    var contentAttrs = elementAttrs.contentAttributes;
    if (!contentAttrs) return;


    element.removeStandartTooltip = function (geom) {
        for (var i = 0; i < geom.childNodes.length; i++) {
            if (geom.childNodes[i].tagName && geom.childNodes[i].tagName.toString().toLowerCase() == "title") {
                geom.removeChild(geom.childNodes[i]);
                break;
            }
        }
    }

    element.findGeoms = function (parent) {
        if (parent.childNodes.length > 0) {
            for (var i = 0; i < parent.childNodes.length; i++) {
                var child = parent.childNodes[i];

                if (child["getAttribute"] && (
                    child.getAttribute("elementtarget") != null ||
                    child.getAttribute("elementvalue") != null ||
                    child.getAttribute("elementseries") != null)) {
                    var geom = child;

                    geom.replaceAllKeys = function (text) {
                        var keys = {
                            target: this.getAttribute("elementtarget"),
                            value: this.getAttribute("elementvalue"),
                            series: this.getAttribute("elementseries")
                        }
                        if (keys.argument == "sti_IsNullValue") keys.argument = "";
                        return jsObject.ReplaceAllKeysInText(text, keys);
                    }

                    geom.getHyperlinkText = function () {
                        return (contentAttrs.interaction && contentAttrs.interaction.hyperlink ? this.replaceAllKeys(contentAttrs.interaction.hyperlink) : "");
                    }

                    geom.getTooltipText = function () {
                        return (contentAttrs.interaction ? jsObject.CorrectTooltipText(this.replaceAllKeys(contentAttrs.interaction.toolTip)) : "");
                    }

                    if (!contentAttrs.interaction || (contentAttrs.interaction && contentAttrs.interaction.onClick != "None")) {
                        geom.style.cursor = "pointer";

                        if (contentAttrs.interaction && contentAttrs.interaction.onClick == "DrillDown" && contentAttrs.interaction.drillDownCurrentLevel >= contentAttrs.interaction.drillDownLevelCount - 1)
                            geom.style.cursor = "default";
                    }

                    if (contentAttrs.interaction && contentAttrs.interaction.onHover == "None") {
                        geom.setAttribute("notShowTooltip", "true");
                    }

                    var interaction = contentAttrs.interaction;

                    //Add hover events
                    if (interaction &&
                        ((interaction.onHover == "ShowHyperlink" && interaction.hyperlink) ||
                            (interaction.onHover == "ShowToolTip" && interaction.toolTip))) {

                        var toolTipText = contentAttrs.interaction.onHover == "ShowHyperlink" ? geom.getHyperlinkText() : geom.getTooltipText();

                        if (contentAttrs.interaction.onHover == "ShowHyperlink") {
                            toolTipText = "<font style=\"color: #0645ad; text-decoration: underline;\">" + toolTipText + "</font>";
                        }

                        geom.setAttribute("_text1", toolTipText);

                        if (!document._stiTooltip)
                            jsObject.CreateCustomTooltip(document, jsObject.controls.mainPanel);

                        jsObject.AddCustomTooltip(element, document);
                    }
                    else {
                        jsObject.AddBrushOver(element, document);
                    }

                    if (interaction.onClick == "OpenHyperlink" && interaction.hyperlink) {
                        geom.style.cursor = "pointer";
                        geom.onclick = function () {
                            if (jsObject.options.appearance.openLinksWindow == "_self")
                                window.location.href = geom.getHyperlinkText() ;
                            else
                                jsObject.openNewWindow(geom.getHyperlinkText() , jsObject.options.appearance.openLinksWindow);
                        }
                    }
                    else if (interaction.onClick == "ShowDashboard" && interaction.drillDownPageKey) {
                        geom.style.cursor = "pointer";
                        geom.onclick = function () {
                            if (document._stiTooltip) {
                                document._stiTooltip.cx = -0.2;
                            }
                            var drillDownParameters = {
                                drillDownPageKey: interaction.drillDownPageKey,
                                value: "",
                                parameters: []
                            }
                            var parameters = interaction.drillDownParameters;
                            if (parameters) {
                                var keys = {
                                    target: this.getAttribute("elementtarget"),
                                    value: this.getAttribute("elementvalue"),
                                    series: this.getAttribute("elementseries")
                                }
                                for (var i = 0; i < parameters.length; i++) {
                                    drillDownParameters.parameters.push({
                                        key: parameters[i].name,
                                        value: (parameters[i].expression ? jsObject.ReplaceAllKeysInText(parameters[i].expression, keys) : "")
                                    });
                                }
                            }
                            jsObject.postInteraction({
                                action: "DashboardDrillDown",
                                drillDownParameters: drillDownParameters,
                                pageNumber: 0
                            });
                        }
                    }
                }

                element.findGeoms(child);
            }
        }
    }

    if (mainContainer) {        
        element.findGeoms(mainContainer);
    }
}

StiJsViewer.prototype.AddInteractionsToHtmlContentDashboardElement = function (element, mainContainer, contentWindow) {
    var jsObject = this;
    var elementAttrs = element.elementAttributes;
    var contentAttrs = elementAttrs.contentAttributes;
    if (!contentAttrs) return;

    //Add interactive events
    var allGeoms = [];
    var findFilteringGeom = false;
    var geomOpacity = elementAttrs.type == "StiRegionMapElement" ? "0.3" : "0.4";

    element.resetAllSelectedGeoms = function () {
        for (var i = 0; i < allGeoms.length; i++) {
            allGeoms[i].isSelected = false;
            allGeoms[i].style.fillOpacity = allGeoms[i].getAttribute("defaultFillOpacity") || "1";
        }
    }

    element.removeStandartTooltip = function (geom) {
        for (var i = 0; i < geom.childNodes.length; i++) {
            if (geom.childNodes[i].tagName && geom.childNodes[i].tagName.toString().toLowerCase() == "title") {
                geom.removeChild(geom.childNodes[i]);
                break;
            }
        }
    }

    element.getFilters = function () {
        var filters = [];
        for (var i = 0; i < allGeoms.length; i++) {
            if (allGeoms[i].isSelected) {
                var elementArgument = allGeoms[i].getAttribute("elementargument");
                var elementValue = allGeoms[i].getAttribute("elementvalue");
                var elementSeries = allGeoms[i].getAttribute("elementseries");
                var value = elementArgument != null ? elementArgument : elementValue;
                if (value == "sti_IsNullValue") value = null;

                if (elementAttrs.type == "StiChartElement") {
                    var argumentColumnPath = contentAttrs.argumentColumnPath;
                    var seriesColumnPath = contentAttrs.seriesColumnPath;
                    var elementIndex = allGeoms[i].getAttribute("elementindex");

                    if (contentAttrs.isBubble) {
                        filters.push({ condition: "EqualTo", value: elementArgument, path: contentAttrs.bubleXColumnPath });
                        filters.push({ condition: "EqualTo", value: elementValue, path: contentAttrs.bubleYColumnPath });
                    }
                    else {
                        if (argumentColumnPath && !seriesColumnPath) {
                            filters.push({ key: elementIndex, path: argumentColumnPath, condition: "EqualTo", value: value });
                        }
                        else if (!argumentColumnPath && seriesColumnPath) {
                            filters.push({ key: elementIndex, path: seriesColumnPath, condition: "EqualTo", value: elementSeries });
                        }
                        else if (argumentColumnPath && seriesColumnPath) {
                            filters.push({ key: elementIndex, path: argumentColumnPath, path2: seriesColumnPath, condition: "PairEqualTo", value: value, value2: elementSeries });
                        }
                    }
                }
                else if (elementAttrs.type == "StiRegionMapElement") {
                    filters.push({ condition: "MapEqualTo", value: value, path: contentAttrs.columnPath });
                }
                else {
                    filters.push({ condition: "EqualTo", value: value, path: contentAttrs.columnPath });
                }
            }
        }
        return filters;
    }

    element.selectGeom = function (geom, multiFilter) {
        if (multiFilter) {
            geom.isSelected = !geom.isSelected;

            for (var i = 0; i < allGeoms.length; i++) {
                allGeoms[i].style.fillOpacity = allGeoms[i].isSelected ? "1" : (allGeoms[i].getAttribute("defaultFillOpacity") != "0" ? geomOpacity : "0");
            }
        }
        else {
            for (var i = 0; i < allGeoms.length; i++) {
                allGeoms[i].isSelected = allGeoms[i] == geom;
                allGeoms[i].style.fillOpacity = allGeoms[i].isSelected ? "1" : (allGeoms[i].getAttribute("defaultFillOpacity") != "0" ? geomOpacity : "0");
            }
        }
    }

    element.updateSelectedGeoms = function () {
        for (var i = 0; i < allGeoms.length; i++) {
            allGeoms[i].setSelected(this.elementAttributes.contentAttributes.filters.some(function (f) {
                var value = allGeoms[i].getAttribute("elementargument");
                if (value == "sti_IsNullValue") value = null;
                return ((f.condition == "MapEqualTo" || f.condition == "EqualTo") && f.value == value)
            }));
        }
    }

    element.checkExistsGeomsByFilter = function (geomElement, filters) {
        var currentFilters = filters || contentAttrs.filters;

        return currentFilters.some(function (f) {
            var elementArgument = geomElement.getAttribute("elementargument");
            var elementValue = geomElement.getAttribute("elementvalue");
            var elementSeries = geomElement.getAttribute("elementseries");
            var value = elementArgument || elementValue;
            var checkValue = value == "sti_IsNullValue" ? f.value == null : ((f.value == null && value == null) ? false : value == f.value);
            var checkValue2 = (elementSeries != null && f.value2 != null) ? elementSeries == f.value2 : true;
            return ((f.condition == "EqualTo" || f.condition == "PairEqualTo" || f.condition == "MapEqualTo") && checkValue && checkValue2);
        });
    }

    element.findGeoms = function (parent) {
        if (parent.childNodes.length > 0) {
            for (var i = 0; i < parent.childNodes.length; i++) {
                var child = parent.childNodes[i];

                //is geom
                if (child["getAttribute"] && (
                    child.getAttribute("elementargument") != null ||
                    child.getAttribute("elementvalue") != null ||
                    child.getAttribute("elementseries") != null ||
                    child.getAttribute("elementident") != null)) {
                    var geom = child;

                    if (geom.style && geom.style.fillOpacity) {
                        geom.setAttribute("defaultFillOpacity", geom.style.fillOpacity);
                    }

                    geom.replaceAllKeys = function (text) {
                        var keys = {
                            value: this.getAttribute("elementvalue"),
                            argument: this.getAttribute("elementargument"),
                            series: this.getAttribute("elementseries"),
                            ident: this.getAttribute("elementident")
                        }
                        if (keys.argument == "sti_IsNullValue") keys.argument = "";
                        return jsObject.ReplaceAllKeysInText(text, keys);
                    }

                    geom.getHyperlinkText = function () {
                        var hyperlinkText = contentAttrs.interaction ? contentAttrs.interaction.hyperlink : "";

                        if (geom.getAttribute("interactionhyperlink") != null)
                            hyperlinkText = geom.getAttribute("interactionhyperlink");

                        return this.replaceAllKeys(hyperlinkText);
                    }

                    geom.getTooltipText = function () {
                        var tooltipText = contentAttrs.interaction ? contentAttrs.interaction.toolTip : "";

                        if (geom.getAttribute("interactiontooltip") != null)
                            tooltipText = geom.getAttribute("interactiontooltip");

                        return jsObject.CorrectTooltipText(this.replaceAllKeys(tooltipText));
                    }

                    if (!contentAttrs.interaction || (contentAttrs.interaction && contentAttrs.interaction.onClick != "None")) {
                        geom.style.cursor = "pointer";

                        if (contentAttrs.interaction && contentAttrs.interaction.onClick == "DrillDown" && contentAttrs.interaction.drillDownCurrentLevel >= contentAttrs.interaction.drillDownLevelCount - 1)
                            geom.style.cursor = "default";
                    }

                    if (contentAttrs.interaction && contentAttrs.interaction.onHover == "None") {
                        geom.setAttribute("notShowTooltip", "true");
                    }

                    //Add hover events
                    if (contentAttrs.interaction && element.frame &&
                        ((contentAttrs.interaction.onHover == "ShowHyperlink" && contentAttrs.interaction.hyperlink) ||
                            (contentAttrs.interaction.onHover == "ShowToolTip" && contentAttrs.interaction.toolTip))) {

                        var toolTipText = contentAttrs.interaction.onHover == "ShowHyperlink" ? geom.getHyperlinkText() : geom.getTooltipText();

                        if (contentAttrs.interaction.onHover == "ShowHyperlink") {
                            toolTipText = "<font style=\"color: #0645ad; text-decoration: underline;\">" + toolTipText + "</font>";
                        }

                        geom.setAttribute("_text1", "");
                        geom.setAttribute("_text2", "");

                        if (toolTipText) {
                            geom.removeAttribute("notShowTooltip");
                            geom.setAttribute("_text1", toolTipText);
                            geom.setAttribute("isCustomTooltip", "true");
                            element.removeStandartTooltip(geom);
                        }

                        if (contentAttrs.interaction.ident == "Chart") {
                            //add chart tooltip
                            var frameDoc = element.frame.contentWindow.document;
                            if (frameDoc) {
                                if (geom.getAttribute("_color")) {
                                    var fillColor = geom.style.fill;
                                    if (fillColor != null && fillColor.toLowerCase().indexOf("rgb") >= 0) {
                                        var colors = fillColor.replace("rgb(", "").replace(")", "").split(",");
                                        if (colors.length >= 3)
                                            fillColor = jsObject.FullColorHex(parseInt(colors[0]), parseInt(colors[1]), parseInt(colors[2]));
                                    }
                                    if (fillColor) geom.setAttribute("_color", fillColor);
                                }
                                if (!frameDoc._stiTooltip) {
                                    jsObject.CreateCustomTooltip(frameDoc);
                                }
                                jsObject.AddCustomTooltip(geom, frameDoc, true);
                            }
                        }
                    }

                    //Add click events
                    geom.onclick = function () {
                        if (contentAttrs.interaction && contentAttrs.interaction.onClick == "OpenHyperlink" && contentAttrs.interaction.hyperlink && !element.isDrillSelectionActivated) {
                            if (jsObject.options.appearance.openLinksWindow == "_self")
                                window.location.href = this.getHyperlinkText();
                            else
                                jsObject.openNewWindow(this.getHyperlinkText(), jsObject.options.appearance.openLinksWindow);
                                
                        }
                        else if (contentAttrs.interaction && contentAttrs.interaction.onClick == "ShowDashboard" && contentAttrs.interaction.drillDownPageKey && !element.isDrillSelectionActivated) {
                            var value = (elementAttrs.type == "StiRegionMapElement" ? this.getAttribute("elementident") : this.getAttribute("elementargument")) || "";
                            if (value == "sti_IsNullValue") value = null;
                            var drillDownParameters = {
                                drillDownPageKey: contentAttrs.interaction.drillDownPageKey,
                                value: value,
                                parameters: []
                            }
                            var parameters = contentAttrs.interaction.drillDownParameters;
                            if (parameters) {
                                for (var i = 0; i < parameters.length; i++) {
                                    drillDownParameters.parameters.push({
                                        key: parameters[i].name,
                                        value: this.replaceAllKeys(parameters[i].expression)
                                    });
                                }
                            }
                            jsObject.postInteraction({
                                action: "DashboardDrillDown",
                                drillDownParameters: drillDownParameters,
                                pageNumber: 0
                            });
                        }
                        else if (contentAttrs.interaction && contentAttrs.interaction.allowUserDrillDown && (contentAttrs.interaction.onClick == "DrillDown" || element.isDrillSelectionActivated)) {
                            if (contentAttrs.interaction.drillDownCurrentLevel < contentAttrs.interaction.drillDownLevelCount - 1) {

                                element.selectGeom(this, element.isDrillSelectionActivated);

                                var filters = element.getFilters();

                                if (element.isDrillSelectionActivated && filters.length == 0) {
                                    element.resetAllSelectedGeoms();
                                }

                                if (!element.isDrillSelectionActivated) {
                                    clearTimeout(element.actionTimer);
                                    element.actionTimer = setTimeout(function () {
                                        jsObject.ApplyDrillDownToDashboardElement(element, filters);
                                    }, 500);
                                }
                            }
                        }
                        else if (!contentAttrs.interaction || (contentAttrs.interaction && contentAttrs.interaction.onClick == "ApplyFilter")) {
                            if (element.afterTransform) {
                                element.afterTransform = false;
                                return;
                            }
                            element.selectGeom(this, element.buttons.multiFilter.isChecked);

                            var filters = element.getFilters();

                            if (element.buttons.multiFilter.isChecked && filters.length == 0) {
                                element.resetAllSelectedGeoms();
                            }

                            for (var i = 0; i < allGeoms.length; i++) {
                                if (element.checkExistsGeomsByFilter(allGeoms[i], filters)) {
                                    allGeoms[i].setSelected(true);
                                }
                            }

                            clearTimeout(element.actionTimer);
                            element.actionTimer = setTimeout(function () {
                                jsObject.ApplyFiltersToDashboardElement(element, filters);
                            }, 500);
                        }
                    }

                    geom.setSelected = function (state) {
                        this.isSelected = state;
                        this.style.fillOpacity = state ? "1" : (this.getAttribute("defaultFillOpacity") != "0" ? geomOpacity : "0");
                    }

                    if (element.checkExistsGeomsByFilter(geom)) {
                        findFilteringGeom = true;
                        geom.setSelected(true);
                    }
                    else {
                        geom.setSelected(false);
                    }

                    allGeoms.push(geom);
                }

                if (child["getAttribute"] && child.getAttribute("isRegionMap") && contentAttrs["showZoomPanel"] !== false) {
                    var path = child;
                    setTimeout(function () {                        
                        element.setupMap(path);
                    }, 100);
                }

                element.findGeoms(child);
            }
        }
    }

    element.setupMap = function (child) {
        var isDark = contentAttrs.isDark;
        var zoomPlusButton = jsObject.SmallButton(null, null, "Dashboards.Actions." + (isDark ? "Dark" : "Light") + ".ZoomPlus.png", null, null, null, elementAttrs.actionColors);
        var zoomMinusButton = jsObject.SmallButton(null, null, "Dashboards.Actions." + (isDark ? "Dark" : "Light") + ".ZoomMinus.png", null, null, null, elementAttrs.actionColors);
        var zoomResetButton = jsObject.SmallButton(null, null, "Dashboards.Actions." + (isDark ? "Dark" : "Light") + ".ResetZoom.png", null, null, null, elementAttrs.actionColors);
        var buttons = [zoomPlusButton, zoomMinusButton, zoomResetButton];

        buttons.forEach(function (button) {
            mainContainer.body.appendChild(button);
            button.style.position = "absolute";
            button.style.zIndex = 10000;
            button.style.left = "15px";
        });

        var transformElement;
        for (var i in child.childNodes[0].childNodes) {
            if (child.childNodes[0].childNodes[i].tagName == "g") {
                transformElement = child.childNodes[0].childNodes[i];
            }
        }
        var transform = transformElement.getAttribute("transform").replace("translate(", "").replace(")", "").split(",");
        var startX = parseFloat(transform[0]);
        var startY = transform[1] ? parseFloat(transform[1]) : 0;
        var translateX = 0;
        var translateY = 0;
        var scale = 1;
        var clientX = 0;
        var clientY = 0;
        var isMouseDown = false;
        var zoomCurrentDistance = 0;
        var zoomPreDistance = 0;
        var zoomStep = 0;

        zoomResetButton.action = function () {
            scale = 1;
            translateX = 0;
            translateY = 0;
            mainContainer.body.transform();
        }

        zoomPlusButton.action = function () {
            scale = Math.min(10, scale + 0.2);
            mainContainer.body.transform();
        }

        zoomMinusButton.action = function () {
            scale = Math.max(0.1, scale - 0.2);
            mainContainer.body.transform();
        }

        if (jsObject.IsTouchDevice() && jsObject.IsMobileDevice()) {
            mainContainer.body.ontouchstart = function (event) {
                if (event.touches && event.touches.length > 1) {
                    zoomCurrentDistance = 0;
                    zoomPreDistance = 0;
                    zoomStep = 0;
                }
                else {
                    clientX = translateX - event.touches[0].pageX;
                    clientY = translateY - event.touches[0].pageY;
                    isMouseDown = true;
                }
            }

            mainContainer.body.ontouchmove = function (event) {
                if (event.touches && event.touches.length > 1) {
                    zoomCurrentDistance = Math.sqrt(Math.pow(event.touches[0].pageX - event.touches[1].pageX, 2) + Math.pow(event.touches[0].pageY - event.touches[1].pageY, 2));
                    zoomStep = zoomCurrentDistance > zoomPreDistance ? zoomStep + 1 : zoomStep - 1;
                    zoomPreDistance = zoomCurrentDistance;

                    if (zoomStep > 20) {
                        zoomStep = 0;
                        scale = Math.min(10, scale + 0.2);
                        mainContainer.body.transform();
                        element.afterTransform = true;
                    }

                    if (zoomStep < -20) {
                        zoomStep = 0;
                        scale = Math.max(0.1, scale - 0.2);
                        mainContainer.body.transform();
                        element.afterTransform = true;
                    }
                }
                else if (isMouseDown) {
                    translateX = event.touches[0].pageX + clientX;
                    translateY = event.touches[0].pageY + clientY;
                    mainContainer.body.transform();
                    element.afterTransform = true;
                }
            }

            contentWindow.addEventListener('touchend', function (e) {
                isMouseDown = false;
                zoomStep = 0;
            });
        }
        else {
            mainContainer.body.onmousedown = function (e) {
                clientX = translateX - e.clientX;
                clientY = translateY - e.clientY;
                isMouseDown = true;
            }

            mainContainer.body.onmousemove = function (e) {
                if (isMouseDown) {
                    translateX = e.clientX + clientX;
                    translateY = e.clientY + clientY;
                    mainContainer.body.transform();
                    element.afterTransform = true;
                }
            }

            contentWindow.addEventListener('mouseup', function (e) {
                isMouseDown = false;
            });
        }

        if ('onwheel' in mainContainer) {
            jsObject.addEvent(mainContainer, "wheel", onWheel);
        } else if ('onmousewheel' in mainContainer) {
            jsObject.addEvent(mainContainer, "mousewheel", onWheel);
        } else {
            jsObject.addEvent(mainContainer, "MozMousePixelScroll", onWheel);
        }

        function onWheel(e) {
            e = e || window.event;
            var delta = e.wheelDelta || e.deltaY || e.detail;
            if (e.wheelDelta == null && e.deltaY != null) delta *= -40; //fix for firefox
            if (delta < 0) {
                zoomMinusButton.action();
            } else {
                zoomPlusButton.action();
            }
        }

        mainContainer.body.onresize = function () {
            zoomPlusButton.style.top = (this.offsetHeight / 2 - zoomMinusButton.offsetHeight / 2 - 5 - zoomPlusButton.offsetHeight) + "px";
            zoomMinusButton.style.top = (this.offsetHeight / 2 - zoomMinusButton.offsetHeight / 2) + "px";
            zoomResetButton.style.top = (this.offsetHeight / 2 + zoomMinusButton.offsetHeight / 2 + 5) + "px";
        }

        mainContainer.body.transform = function () {
            var bbox = transformElement.getBBox();
            transformElement.setAttribute("transform", "translate(" + (startX + translateX + (1 - scale) * bbox.width / 2) + ", " + (startY + translateY + (1 - scale) * bbox.height / 2) + ") scale(" + scale + ")");
            mainContainer._stiTooltip.cx = -0.2;
        }

        try {
            mainContainer.body.style.height = mainContainer.body.style.width = "100%";
            mainContainer.body.onresize();
        }
        catch (e) { }
    }

    if (mainContainer) {
        element.findGeoms(mainContainer);
        if (!findFilteringGeom) element.resetAllSelectedGeoms();
    }
}

StiJsViewer.prototype.AddRefreshTimerToDashboardElement = function (element) {
    var jsObject = this;
    var elementName = element.elementAttributes.name;
    if (!jsObject.service.elementRefreshTimers[elementName]) {
        jsObject.service.elementRefreshTimers[elementName] = setInterval(function () {
            var isRequestInProcess = jsObject.service.isRequestInProcess;
            jsObject.postAjax(jsObject.getActionRequestUrl(jsObject.options.requestUrl, jsObject.options.actions.viewerEvent),
                {
                    action: "DashboardGetSingleElementContent",
                    elementNameForRefreshing: elementName
                },
                function (answer) {
                    if (answer) {
                        try {
                            var element = jsObject.controls.reportPanel.getDashboardElementByName(elementName);
                            if (!element || !element.offsetWidth || !element.offsetHeight) {
                                clearInterval(jsObject.service.elementRefreshTimers[elementName]);
                                delete jsObject.service.elementRefreshTimers[elementName];
                            }
                            var elementAttributes = JSON.parse(jsObject.options.server.useCompression ? StiGZipHelper.unpack(answer) : answer);
                            if (element && elementAttributes) {
                                element.elementAttributes = elementAttributes;
                                jsObject.UpdateButtonsPanel(element);
                                jsObject.UpdateFiltersStringPanel(element);
                                jsObject.InsertContentToDashboardElement(element);
                            }
                        } catch (e) { };
                    }
                });
            jsObject.service.isRequestInProcess = isRequestInProcess;
        }, 1000);
    }
}

StiJsViewer.prototype.AddInteractionsToDashboardElement = function (element) {
    var jsObject = this;
    var elementAttrs = element.elementAttributes;
    var contentAttrs = elementAttrs.contentAttributes;
    if (!contentAttrs) return;
    var interaction = contentAttrs.interaction;

    if (interaction) {
        if ((interaction.onHover == "ShowToolTip" && interaction.toolTip) || (interaction.onHover == "ShowHyperlink" && interaction.hyperlink)) {
            var toolTipText = interaction.onHover == "ShowToolTip" ? interaction.toolTip : interaction.hyperlink;
            toolTipText = jsObject.CorrectTooltipText(jsObject.ReplaceAllKeysInText(toolTipText, {}));

            if (interaction.onHover == "ShowHyperlink") {
                toolTipText = "<font style=\"color: #0645ad; text-decoration: underline;\">" + toolTipText + "</font>";
            }

            element.setAttribute("_text1", toolTipText);

            if (!document._stiTooltip)
                jsObject.CreateCustomTooltip(document, jsObject.controls.mainPanel);

            jsObject.AddCustomTooltip(element, document);
        }

        if (interaction.onClick == "OpenHyperlink" && interaction.hyperlink) {
            element.style.cursor = "pointer";
            element.onclick = function () {
                if (jsObject.options.appearance.openLinksWindow == "_self")
                    window.location.href = interaction.hyperlink;
                else
                    jsObject.openNewWindow(interaction.hyperlink, jsObject.options.appearance.openLinksWindow);
            }
        }       
        else if (interaction.onClick == "ShowDashboard" && interaction.drillDownPageKey) {
            element.style.cursor = "pointer";
            element.onclick = function () {
                if (document._stiTooltip) {
                    document._stiTooltip.cx = -0.2;
                }
                var drillDownParameters = {
                    drillDownPageKey: interaction.drillDownPageKey,
                    value: "",
                    parameters: []
                }
                var parameters = interaction.drillDownParameters;
                if (parameters) {
                    var keys = {};
                    if (elementAttrs.type == "StiTextElement" && contentAttrs.plainText != null) {
                        keys.value = contentAttrs.plainText;
                    }
                    for (var i = 0; i < parameters.length; i++) {
                        drillDownParameters.parameters.push({
                            key: parameters[i].name,
                            value: (parameters[i].expression ? jsObject.ReplaceAllKeysInText(parameters[i].expression, keys) : "")
                        });
                    }
                }
                jsObject.postInteraction({
                    action: "DashboardDrillDown",
                    drillDownParameters: drillDownParameters,
                    pageNumber: 0
                });
            }
        }
    }
}

StiJsViewer.prototype.ReplaceAllKeysInText = function (text, keys) {
    var value = keys.value || "";
    var argument = keys.argument || "";
    var series = keys.series || "";
    var ident = keys.ident || "";
    var taget = keys.taget || "";

    if (text != null) {
        text = text.replace(/{value}/g, value).replace(/{Value}/g, value);
        text = text.replace(/{argument}/g, argument).replace(/{Argument}/g, argument);
        text = text.replace(/{series}/g, series).replace(/{Series}/g, series);
        text = text.replace(/{ident}/g, ident).replace(/{Ident}/g, ident);
        text = text.replace(/{target}/g, ident).replace(/{Target}/g, ident);

        if (this.reportParams.variablesValues) {
            var currVarsValues = this.controls.parametersPanel ? this.controls.parametersPanel.getParametersValues() : {};
            for (var varName in this.reportParams.variablesValues) {
                var varValue = currVarsValues[varName] != null ? currVarsValues[varName] : this.reportParams.variablesValues[varName];
                text = text.replace(new RegExp("{" + varName + "}", 'g'), (varValue || ""));
            }
        }

        return text;
    }
    
    return "";
}

StiJsViewer.prototype.CorrectTooltipText = function (text) {
    if (text != null) {
        //replace to supported html tags
        text = text.replace(/<font-color/g, '<font color').replace(/<\/font-color>/g, '</font>');

        var sizes = ["8", "10", "12", "14", "18", "24", "36"];
        var newString = "style=\"font-size:";
        var oldString = "size=\"";

        while (text.indexOf("size=") >= 0) {
            var startIndex = text.indexOf("size=");
            text = text.replace(oldString, newString);
            startIndex += newString.length;

            var firstPart = text.substring(0, startIndex);
            var tempPart = text.substring(startIndex);
            var middlePart = tempPart.substring(0, tempPart.indexOf("\""));
            var lastPart = tempPart.substring(tempPart.indexOf("\""));

            if (parseInt(middlePart) <= 7 && parseInt(middlePart) > 0) {
                text = firstPart + sizes[parseInt(middlePart) - 1] + "pt" + lastPart;
            }
            else {
                text = firstPart + middlePart + "pt" + lastPart;
            }
        }
        return text;
    }
    return "";
}

StiJsViewer.prototype.CreateCustomTooltip = function (doc, owner) {
    var table = this.CreateHTMLTable();
    table.style.position = "absolute";
    table.style.opacity = "0";
    table.style.background = "white";
    table.style.padding = "5px";
    table.style.border = "1px solid #bebebe";
    table.style.fontFamily = "Arial";
    table.style.fontSize = "12px";
    table.style.color = "#111111";
    table.style.zIndex = "20000";
    table.style.pointerEvents = "none";
    table._text1 = table.addCell();
    table._text1.style.paddingTop = "3px";
    table._text2 = table.addCellInNextRow();
    table._text2.style.paddingTop = "3px";

    doc._stiTooltip = table;

    if (owner)
        owner.appendChild(table);
    else
        doc.body.appendChild(table);

    setInterval(function () {
        var t = doc._stiTooltip;
        var op = parseFloat(t.style.opacity);
        if ((t.cx > 0 && op < 1) || (t.cx < 0 && op > 0)) {
            op += t.cx;
            op = Math.min(1, Math.max(0, op));
            t.style.opacity = op;
        }
    }, 50);
}

StiJsViewer.prototype.AddBrushOver = function (el, doc) {
    var i = 0;
    this.addEvent(el, "mouseover", function (event) {
        if (event.target.getAttribute("_brushover") == null) {
            return;
        }
        if (!event.target.getAttribute("_color")) {
            event.target.setAttribute("_color", event.target.style.fill);
        }
        event.target.style.fill = event.target.getAttribute("_brushover");
    });

    this.addEvent(el, "mouseout", function (event) {
        var target = event && event.target ? event.target : el;
        var color = target.getAttribute("_color");
        if (color) target.style.fill = color;
    });
}

StiJsViewer.prototype.AddCustomTooltip = function (el, doc, inFrame) {
    var jsObject = this;
    var t = doc._stiTooltip;

    el.getColor = function (colorStyle) {
        if (colorStyle.indexOf("rgb") == 0) {
            var colors = colorStyle.replace("rgb(", "").replace(")", "").split(",");
            var r = parseInt(colors[0]);
            var g = parseInt(colors[1]);
            var b = parseInt(colors[2]);
            return "#" + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1);
        }
        return colorStyle;
    }

    el.onmouseover = function (event) {
        if (event.target.getAttribute("notShowTooltip") == "true")
            return;

        clearTimeout(t.hiddenTimer);

        var text = event.target.getAttribute("_text1") || this.getAttribute("_text1");

        if (text == null) {
            //find on the top levels
            var level = 0;
            var currEl = event.target;
            while (currEl.parentElement != null && level < 3) {
                if (currEl.parentElement.getAttribute("_text1")) {
                    text = currEl.parentElement.getAttribute("_text1");
                    break;
                }
                else {
                    currEl = currEl.parentElement;
                }
                level++;
            }
        }

        if (!text) return;

        if (text.toLowerCase().indexOf("<a ") >= 0) {
            t.style.pointerEvents = "auto";
            text = text.replace(/<a /g, "<a target='_blank' ");
        }

        t.cx = 0.1;
        t._text1.innerHTML = text;

        event.target.setAttribute("_color", event.target.style.fill);
        
        var _color = el.getColor(event.target.style.fill);
        if (_color) event.target.style.fill = jsObject.LightenDarkenColor(_color, -35);
        if (event.target.getAttribute("_brushover")) {
            event.target.style.fill = event.target.getAttribute("_brushover");
        }

        if (inFrame) {
            var cx = Math.max(event.pageX + 1 + t.offsetWidth - window.outerWidth + 10, 0);
            var cy = Math.max(event.pageY + 1 + t.offsetHeight - window.outerHeight + 10, 0);
            cx = Math.max(cx, event.pageX + 1 + t.offsetWidth - doc.body.offsetWidth);
            cy = Math.max(cy, event.pageY + 1 + t.offsetHeight - getDocHeight());
            t.style.left = (event.pageX + 1 - cx) + "px";
            t.style.top = (event.pageY + 1 - cy) + "px";
        }
        else {
            var point = jsObject.FindMousePosOnMainPanel(event);
            t.style.left = (point.xPixels + 5) + "px";
            t.style.top = (point.yPixels + 5) + "px";
        }
    }

    function getDocHeight() {
        var D = document;
        return Math.max(
            Math.max(D.body.scrollHeight, D.documentElement.scrollHeight),
            Math.max(D.body.offsetHeight, D.documentElement.offsetHeight),
            Math.max(D.body.clientHeight, D.documentElement.clientHeight)
        );
    }

    el.onmouseout = function (event) {
        var target = event && event.target ? event.target : el;
        var color = target.getAttribute("_color");
        if (color) target.style.fill = color;

        t.hiddenTimer = setTimeout(function () {
            if (!t.isOver) t.cx = -0.2;
        }, 500);
    }

    t.onmouseover = function () {
        this.isOver = true;
        clearTimeout(t.hiddenTimer);
    }

    t.onmouseout = function () {
        this.isOver = false;
        el.onmouseout();
    }

    t.onclick = function () {
        this.onmouseout();
    }
}