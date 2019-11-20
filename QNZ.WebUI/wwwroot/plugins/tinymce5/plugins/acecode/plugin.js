(function () {
    'use strict';

    var global = tinymce.util.Tools.resolve('tinymce.PluginManager');

    var setContent = function (editor, html) {
        editor.focus();
        editor.undoManager.transact(function () {
            editor.setContent(html);
        });
        editor.selection.setCursorLocation();
        editor.nodeChanged();
    };
    var getContent = function (editor) {
        return editor.getContent({ source_view: true });
    };
    var Content = {
        setContent: setContent,
        getContent: getContent
    };

    var open = function (editor) {
        var editorContent = Content.getContent(editor);
      
        editor.windowManager.openUrl({
            title: 'Source Code',
            url: '/plugins/tinymce5/plugins/acecode/source.html',
            buttons: [
                {
                    type: "custom",
                    name: "insert-and-close",
                    text: "确定",
                    primary: true,
                    align: "end"
                },
                {
                    type: "cancel",
                    name: "cancel",
                    text: "关闭"
                }
            ],
            width: 1200,
            height: 700,
            onAction: function (instance, trigger) {
                instance.sendMessage({
                    mceAction: "customInsertAndClose",
                    content: editorContent
                });
            },
            onMessage: function (instance, data) {
                console.log("111" + data);
                // we can do something here with the
                // instance and the data of the message
                switch (data.mceAction) {
                    case 'insertContent':
                        // run code for inserting content
                        break;
                    case 'replaceContent':
                        // run code for replacing the content
                        break;
                    //
                    // etc...
                    //
                }
            }
        });


    };
    var Dialog = { open: open };


    var register = function (editor) {
        editor.addCommand('mceCodeEditor', function () {
            Dialog.open(editor);
        });
    };
    var Commands = { register: register };

    var register$1 = function (editor) {
        editor.ui.registry.addButton("code", {
            text: "Open Dialog",
            icon: "frame",
            onAction: () => {
                return Dialog.open(editor);
            }
        });

         editor.ui.registry.addMenuItem('code', {
            icon: 'sourcecode',
            text: 'Source code',
            onAction: function () {
                return Dialog.open(editor);
            }
        });

        editor.addCommand("iframeCommand", function (ui, value) {
            editor.setContent(
                `${value}`
            );
        });

        //editor.ui.registry.addButton('code', {
        //    icon: 'sourcecode',
        //    tooltip: 'Source code',
        //    onAction: function () {
        //        return Dialog.open(editor);
        //    }
        //});
        //editor.ui.registry.addMenuItem('code', {
        //    icon: 'sourcecode',
        //    text: 'Source code',
        //    onAction: function () {
        //        return Dialog.open(editor);
        //    }
        //});
    };
    var Buttons = { register: register$1 };

    function Plugin() {
        global.add('acecode', function (editor) {
            Commands.register(editor);
            Buttons.register(editor);
            return {};
        });
    }

    Plugin();

}());
