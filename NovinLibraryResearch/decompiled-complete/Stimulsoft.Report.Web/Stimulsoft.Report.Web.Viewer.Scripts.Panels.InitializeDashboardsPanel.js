
StiJsViewer.prototype.InitializeDashboardsPanel = function () {
    var dashboardsPanel = document.createElement("div");
    this.controls.dashboardsPanel = dashboardsPanel;
    this.controls.mainPanel.appendChild(dashboardsPanel);
    dashboardsPanel.style.fontFamily = this.options.toolbar.fontFamily;
    if (this.options.toolbar.fontColor != "") dashboardsPanel.style.color = this.options.toolbar.fontColor;
    dashboardsPanel.className = "stiJsViewerToolBar";
    if (this.options.toolbar.displayMode == "Separated") dashboardsPanel.className += " stiJsViewerToolBarSeparated";
    var jsObject = dashboardsPanel.jsObject = this;
    dashboardsPanel.style.display = "none";
    dashboardsPanel.visible = false;
    dashboardsPanel.buttons = [];

    var innerPanel = document.createElement("div");
    dashboardsPanel.appendChild(innerPanel);
    if (this.options.toolbar.displayMode == "Simple") innerPanel.style.paddingTop = "2px";

    // Inner content panel
    var panelTable = this.CreateHTMLTable();
    innerPanel.appendChild(panelTable);
    dashboardsPanel.panelTable = panelTable;
    panelTable.className = "stiJsViewerToolBarTable";
    if (this.options.toolbar.displayMode == "Separated") panelTable.style.border = "0px";
    panelTable.style.margin = 0;
    panelTable.style.boxSizing = "border-box";

    // Left and right content tables
    var cell1 = panelTable.addCell();
    var cell2 = panelTable.addCell();
    var mainCell = (!this.options.appearance.rightToLeft) ? cell1 : cell2;
    var actionsCell = (!this.options.appearance.rightToLeft) ? cell2 : cell1;
    mainCell.style.width = "100%";
    var mainTable = this.CreateHTMLTable();
    var actionsTable = this.CreateHTMLTable();
    mainCell.appendChild(mainTable);
    actionsCell.appendChild(actionsTable);
    mainTable.setAttribute("align", this.options.appearance.rightToLeft ? "right" : (this.options.toolbar.alignment == "default" ? "left" : this.options.toolbar.alignment));
    actionsTable.style.marginRight = "2px";
    dashboardsPanel.actionsTable = actionsTable;
    var imagesPath = "Dashboards.Actions.Light.";

    // Refresh button
    if (this.options.toolbar.showRefreshButton && this.options.toolbar.visible) {
        var buttonRefresh = this.SmallButton("RefreshDashboard", null, imagesPath + "Refresh.png", [this.collections.loc["Refresh"], this.helpLinks["DashboardToolbar"], { top: "auto" }]);
        buttonRefresh.jsObject = this;        
        buttonRefresh.style.margin = "2px 0 2px 2px";
        buttonRefresh.action = function () {
            jsObject.postAction("Refresh");
        };
        actionsTable.addCell(buttonRefresh);
    }

    // Parameters button
    if (this.options.toolbar.showParametersButton && this.options.toolbar.visible) {
        var buttonParameters = this.SmallButton("ParametersDashboard", null, imagesPath + "Parameters.png", [this.collections.loc["Parameters"], this.helpLinks["DashboardToolbar"], { top: "auto" }]);
        buttonParameters.jsObject = this;
        buttonParameters.style.margin = "2px 0 2px 2px";
        buttonParameters.style.display = "none";
        buttonParameters.action = function () {
            jsObject.postAction("Parameters");

            setTimeout(function () { jsObject.postInteraction({ action: "Variables", variables: jsObject.controls.parametersPanel.getParametersValues() }); },
                jsObject.options.isMobileDevice ? 500 : 0);
        };
        actionsTable.addCell(buttonParameters);
    }

    // Open button
    if (this.options.toolbar.showOpenButton && this.options.toolbar.visible) {
        var buttonOpen = this.SmallButton("OpenDashboard", null, imagesPath + "Open.png", [this.collections.loc["Open"], this.helpLinks["DashboardToolbar"], { top: "auto" }]);
        buttonOpen.style.margin = "2px 0 2px 2px";
        buttonOpen.action = function () {
            var openDashboardDialog = jsObject.InitializeOpenDialog("openDashboardDialog", function (fileName, filePath, content) {
                jsObject.postOpen(fileName, content);
            }, ".mdc,.mdz,.mdx,.mrt,.mrz,.mrx");
            openDashboardDialog.action();
        };
        actionsTable.addCell(buttonOpen);
    }

    // Edit button
    if (this.options.toolbar.showDesignButton && this.options.toolbar.visible) {
        var buttonEdit = this.SmallButton("EditDashboard", null, imagesPath + "Edit.png", [this.collections.loc["Edit"], this.helpLinks["DashboardToolbar"], { top: "auto" }]);
        buttonEdit.style.margin = "2px 0px 2px 2px";
        buttonEdit.action = function () {
            jsObject.postDesign();
        };
        actionsTable.addCell(buttonEdit);
    }

    // FullScreen button
    if (this.options.toolbar.showFullScreenButton && this.options.toolbar.visible) {
        var buttonFullScreen = this.SmallButton("FullScreenDashboard", jsObject.collections.loc.Close, imagesPath + "CloseFullScreen.png", [this.collections.loc["FullScreen"], this.helpLinks["DashboardToolbar"], { top: "auto" }]);
        buttonFullScreen.style.margin = "2px 0 2px 2px";
        buttonFullScreen.action = function () {
            jsObject.postAction("FullScreen");
            jsObject.postAction("GetPages");
        };
        buttonFullScreen.setFullScreenState = function (state) {
            this.caption.style.display = state ? "" : "none";
            if (jsObject.options.isTouchDevice) buttonFullScreen.style.width = state ? "auto" : "32px";
            this.imageName = state ? this.imageName.replace("Open", "Close") : this.imageName.replace("Close", "Open");
            this.image.src = jsObject.collections.images[this.imageName];
        }
        actionsTable.addCell(buttonFullScreen);
    }    

    // Export Dashboard menu button
    if (this.options.toolbar.showSaveButton && this.options.toolbar.visible) {
        var buttonExport = this.SmallButton("ExportDashboard", null, imagesPath + "Save.png", [this.collections.loc["Save"], this.helpLinks["DashboardToolbar"], { top: "auto" }]);
        buttonExport.style.margin = "2px 0 2px 2px";
        buttonExport.action = function () {
            var menuStyle = this.isDarkStyle ? "stiJsViewerDbsDarkMenu" : "stiJsViewerDbsLightMenu";
            var saveMenu = jsObject.InitializeSaveDashboardMenu(menuStyle + "Item", menuStyle, false);
            saveMenu.changeVisibleState(true, this);
            if (this.previewSettings) {
                if (!this.previewSettings.dashboardShowExports) {
                    var itemNames = ["separator", "Pdf", "Excel2007", "Data", "Image", "Html"];
                    for (var i = 0; i < itemNames.length; i++) {
                        if (saveMenu.items[itemNames[i]]) saveMenu.items[itemNames[i]].style.display = "none";
                    }
                }
                if (!this.previewSettings.dashboardShowReportSnapshots) {
                    var itemNames = ["separator", "Document"];
                    for (var i = 0; i < itemNames.length; i++) {
                        if (saveMenu.items[itemNames[i]]) saveMenu.items[itemNames[i]].style.display = "none";
                    }
                }
            }
        };
        actionsTable.addCell(buttonExport);
    }    
    // Panel methods

    dashboardsPanel.changeVisibleState = function (state) {
        this.visible = state;
        this.style.display = state ? "" : "none";
    }    

    var dbsToolButtons = ["RefreshDashboard", "ExportDashboard", "FullScreenDashboard", "OpenDashboard", "EditDashboard", "ParametersDashboard"];

    dashboardsPanel.update = function (dashboards, previewSettings) {
        if (!dashboards || dashboards.length == 0) return;
        this.clear();
        this.dashboardsCount = 0;
        this.reportsCount = 0;
        this.buttons = [];
        var firstButton = null;
        
        for (var index = 0; index < dashboards.length; index++) {
            var info = dashboards[index];
            var button = jsObject.DashboardPanelButton(dashboardsPanel, "button" + info.name, info.alias);
            if (!firstButton && !info.isNestedPage) firstButton = button;
            var showToolbar = previewSettings ? previewSettings.dashboardToolBar : jsObject.options.toolbar.visible;
            button.style.display = showToolbar && !info.isNestedPage ? "" : "none";
            this.buttons.push(button);
            
            button.reportParams = firstButton === button ? jsObject.reportParams : {};
            button.reportParams.type = info.type;
            button.reportParams[jsObject.options.toolbar.zoom < 0 ? "autoZoom" : "zoom"] = jsObject.options.toolbar.zoom;
            button.reportParams.pageNumber = info.type == "Dashboard" ? info.index : 0;
            button.reportParams.originalPageNumber = info.index;
            button.reportParams.viewMode = jsObject.reportParams.viewMode;

            if (info.valid) button.isValid = true;

            if (info.type == "Dashboard")
                this.dashboardsCount++;
            else
                this.reportsCount++;

            button.action = function () {
                if (!this.closeButtonAction) {
                    this.select();
                    jsObject.reportParams = this.reportParams;
                    jsObject.reportParams.dashboardDrillDownGuid = null;
                    jsObject.postAction("Refresh");
                }
            };

            mainTable.addCell(button);
        }

        if (firstButton) firstButton.select();
        
        var dbsMode = this.dashboardsCount > 0 && this.reportsCount == 0;
        dashboardsPanel.style.background = dbsMode ? "transparent" : "";
        dashboardsPanel.style.borderColor = dbsMode ? "transparent" : "";
        panelTable.style.background = dbsMode ? "transparent" : "";
        panelTable.style.borderColor = dbsMode ? "transparent" : "";

        if (this.dashboardsCount == 1 && this.buttons.length == 1) {
            this.buttons[0].style.display = "none";
        }

        for (var i = 0; i < this.buttons.length; i++) {
            this.buttons[i].style.borderColor = !dbsMode ? "transparent" : "";
            if (jsObject.options.toolbar.displayMode == "Separated") {
                this.buttons[i].style.height = dbsMode ? (jsObject.options.isTouchDevice ? "28px" : "23px") : "28px";
            }
        }
        
        for (var i = 0; i < dbsToolButtons.length; i++) {
            var button = jsObject.controls.buttons[dbsToolButtons[i]];
            if (button && jsObject.options.toolbar.displayMode == "Separated") {
                button.style.height = dbsMode ? (jsObject.options.isTouchDevice ? "28px" : "23px") : "28px";
                if (!button.caption) button.style.width = button.style.height;
                button.innerTable.style.width = "100%";
                button.imageCell.style.textAlign = "center";
            }
        }

        if (previewSettings) {
            var buttons = jsObject.controls.buttons;
            if (buttons.RefreshDashboard) buttons.RefreshDashboard.style.display = previewSettings.dashboardToolBar && previewSettings.dashboardRefreshButton ? "" : "none";
            if (buttons.OpenDashboard) buttons.OpenDashboard.style.display = previewSettings.dashboardToolBar && previewSettings.dashboardOpenButton ? "" : "none";
            if (buttons.EditDashboard) buttons.EditDashboard.style.display = previewSettings.dashboardToolBar && previewSettings.dashboardEditButton ? "" : "none";
            if (buttons.FullScreenDashboard) buttons.FullScreenDashboard.style.display = previewSettings.dashboardToolBar && previewSettings.dashboardFullScreenButton ? "" : "none";
            if (buttons.ParametersDashboard) buttons.ParametersDashboard.allowToShow = previewSettings.dashboardToolBar;
            if (buttons.ExportDashboard) {
                buttons.ExportDashboard.style.display = previewSettings.dashboardToolBar && previewSettings.dashboardMenuButton && ((previewSettings.dashboardShowReportSnapshots && !jsObject.options.jsMode) || previewSettings.dashboardShowExports) ? "" : "none";
                buttons.ExportDashboard.previewSettings = previewSettings;
            }
        }

        var hash = window.location.hash ? window.location.hash.substring(1) : null;
        if (hash != null) {
            for (var i = 0; i < this.buttons.length; i++) {
                if (this.buttons[i].name == "button" + hash) {
                    this.buttons[i].action();
                    break;
                }
            }
        }

        this.changeVisibleState(true);
        jsObject.updateLayout();
    }

    dashboardsPanel.addDrillDownButton = function (dashboardDrillDownGuid, drillDownGuid, drillDownParameters, previewSettings, drillDownReportFileName) {

        //select if button already exists
        for (var i = 0; i < this.buttons.length; i++) {
            var reportParams = this.buttons[i].reportParams;
            if ((dashboardDrillDownGuid && reportParams.dashboardDrillDownGuid == dashboardDrillDownGuid) || (drillDownGuid && reportParams.drillDownGuid == drillDownGuid)) {
                this.buttons[i].select();
                jsObject.reportParams = reportParams;
                return;
            }
        }

        //create new if button not exists
        var caption = drillDownReportFileName || jsObject.collections.loc["Report"] || "Report";

        if (!drillDownGuid && dashboardDrillDownGuid && drillDownParameters && drillDownParameters.length > 0) {
            var dashboardDrillDownParameters = drillDownParameters[drillDownParameters.length - 1];

            //dbs drilldown
            caption = dashboardDrillDownParameters.value;

            if (dashboardDrillDownParameters) {
                for (var i = 0; i < dashboardDrillDownParameters.parameters.length; i++) {
                    var key = dashboardDrillDownParameters.parameters[i].key;
                    if (key && key.toLowerCase() == "title") {
                        caption = dashboardDrillDownParameters.parameters[i].value;
                        break;
                    }
                }
            }
            if (!caption) {
                caption = drillDownReportFileName || jsObject.collections.loc["Dashboard"];
            }
        }

        var dbsMode = this.dashboardsCount > 0 && this.reportsCount == 0;
        var button = jsObject.DashboardPanelButton(dashboardsPanel, "button" + (dashboardDrillDownGuid || drillDownGuid), caption, true);
        this.buttons.push(button);

        if (jsObject.options.toolbar.displayMode == "Separated") {
            button.style.height = dbsMode ? (jsObject.options.isTouchDevice ? "28px" : "23px") : "28px";
        }

        button.reportParams = {
            type: dashboardDrillDownGuid ? "Dashboard" : "Report",
            pageNumber: 0,
            viewMode: jsObject.reportParams.viewMode,
            drillDownGuid: drillDownGuid,
            dashboardDrillDownGuid: dashboardDrillDownGuid,
            drillDownParameters: drillDownParameters
        };

        button.reportParams[jsObject.options.toolbar.zoom < 0 ? "autoZoom" : "zoom"] = jsObject.options.toolbar.zoom;

        jsObject.reportParams = button.reportParams;

        if (this.selectedButton) button.isValid = this.selectedButton.isValid;

        button.action = function () {
            if (!this.closeButtonAction) {
                button.select();
                jsObject.reportParams = this.reportParams;
                jsObject.postInteraction({ action: this.reportParams.drillDownGuid ? "DrillDown" : "DashboardDrillDown" });
            }
        };

        mainTable.addCell(button);
        button.select();

        var showToolbar = previewSettings ? previewSettings.dashboardToolBar : jsObject.options.toolbar.visible;
        button.style.display = showToolbar ? "" : "none";
    }

    dashboardsPanel.updateButtonsStyles = function (styleColors, hasReports) {
        if (hasReports) styleColors = null;
        var isDarkStyle = styleColors && styleColors.isDarkStyle;
        for (var i = 0; i < this.buttons.length; i++) {
            var button = this.buttons[i];
            button.isDarkStyle = isDarkStyle;
            button.applyStyleColors(styleColors);

            //correct styles for AliceBlue style
            if (styleColors && styleColors.styleName == "AliceBlue") {
                var styleColors_ = jsObject.copyObject(styleColors);
                styleColors_.foreColor = "#e4ffff";
                styleColors_.hotForeColor = "#e4ffff";
                styleColors_.hotSelectedForeColor = "#e4ffff";
                button.applyStyleColors(styleColors_);
            }

            //correct styles for close button
            var closeButton = button.closeButton;
            if (closeButton && closeButton.image) {
                var styleColors__ = null;
                if (styleColors) {
                    styleColors__ = jsObject.copyObject(styleColors);
                    styleColors__["backColor"] = "transparent";
                }
                closeButton.applyStyleColors(styleColors__);
                closeButton.imageName = button.isDarkStyle ? closeButton.imageName.replace(".Light.", ".Dark.") : closeButton.imageName.replace(".Dark.", ".Light.");
                closeButton.image.src = jsObject.collections.images[closeButton.imageName];
            }
        }
        for (var i = 0; i < dbsToolButtons.length; i++) {
            var button = jsObject.controls.buttons[dbsToolButtons[i]];
            if (button) {
                button.isDarkStyle = isDarkStyle;
                button.applyStyleColors(styleColors);
                if (button.image) {
                    button.imageName = button.isDarkStyle ? button.imageName.replace(".Light.", ".Dark.") : button.imageName.replace(".Dark.", ".Light.");
                    button.image.src = jsObject.collections.images[button.imageName];
                }
            }
        }
        if (jsObject.controls.parametersPanel && jsObject.controls.parametersPanel.visible && styleColors) {
            jsObject.controls.parametersPanel.style.color = styleColors.styleName == "AliceBlue" ? "#e4ffff" : styleColors.foreColor;
            jsObject.controls.parametersPanel.mainButtons.submit.style.color = jsObject.controls.parametersPanel.mainButtons.reset.style.color = "#444444";
        }
    }

    dashboardsPanel.clear = function () {
        mainTable.clearRow();
        this.selectedButton = null;
    }
}

StiJsViewer.prototype.DashboardPanelButton = function (dashboardsPanel, name, caption, showCloseButton) {
    var button = this.SmallButton(name, caption);
    button.panel = this;
    button.style.margin = "2px 1px 2px 2px";

    button.select = function () {
        if (dashboardsPanel.selectedButton)
            dashboardsPanel.selectedButton.setSelected(false);
        dashboardsPanel.selectedButton = this;
        this.setSelected(true);
    }

    if (showCloseButton) {
        var closeButton = this.SmallButton(null, null, "Dashboards.Actions.Light.Close.png");
        button.closeButton = closeButton;
        closeButton.style.display = "inline-block";
        closeButton.style.padding = "0";
        closeButton.style.margin = "3px 2px 0 0";
        closeButton.imageCell.style.padding = 0;
        closeButton.imageCell.style.textAlign = "center";
        closeButton.style.width = this.options.isTouchDevice ? "22px" : "16px";
        closeButton.style.height = closeButton.style.width;
        button.innerTable.addCell(closeButton);

        closeButton.action = function () {
            button.closeButtonAction = true;
            if (dashboardsPanel.buttons.indexOf(button) >= 0) {
                dashboardsPanel.buttons.splice(dashboardsPanel.buttons.indexOf(button), 1);
            }
            button.parentElement.parentElement.removeChild(button.parentElement);
            if (dashboardsPanel.selectedButton == button && dashboardsPanel.buttons.length > 0) {
                for (var i = 0; i < dashboardsPanel.buttons.length; i++) {
                    if (dashboardsPanel.buttons[i].style.display == "") {
                        dashboardsPanel.buttons[i].action();
                        break;
                    }
                }
            }
        };

        closeButton.onmouseenter = function (event) {
            button.onmouseoutAction();
            this.onmouseoverAction();
            if (event) event.stopPropagation();
        }
    }

    return button;
}