using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace TeacherClient
{
    public class MyLocalizationManager: LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                case "ColorSelectorStandardPaletteHeaderText":
                    return "常用色";

                case "ColorSelectorMainPaletteHeaderText":
                    return "主题颜色";
                case "ColorSelectorAutomaticColorText":
                    return "自动";
                case "ColorSelectorRecentColorsHeaderText":
                    return "最近";
                case "Documents_TableSizePicker_InsertTable":
                    return "插入表格";
                case "Documents_TableSizePicker_TableSize":
                    return "{0}x{1} 表格";
                case "Documents_InsertTableDialog_Header":
                    return "插入表格";
                case "Documents_InsertTableDialog_TableSize":
                    return "表格大小";
                case "Documents_InsertTableDialog_NumberOfColumns":
                    return "列数";
                case "Documents_InsertTableDialog_NumberOfRows":
                    return "行数";
                case "Ok":
                    return "确定";
                case "Cancel":
                    return "取消";
                case "Minimize":
                    return "最小值";
                case "Restore":
                    return "还原";
                case "Maximize":
                    return "最大值";
                case "Close":
                    return "关闭";
                //case "Documents_RadRichTextBox_HyperlinkToolTipFormatString":
                //    return "";
                case "Documents_TableStylesGallery_CustomTablesHeader":
                    return "自定义";
                //case "Documents_TableStylesGallery_PlainTablesHeader":
                //    return "";
                case "Documents_InsertHyperlinkDialog_Header":
                    return "插入超链接";
                case "Documents_InsertHyperlinkDialog_TextToDisplay":
                    return "显示文本";
                case "Documents_InsertHyperlinkDialog_TargetFrame":
                    return "目标框架";
                case "Documents_InsertHyperlinkDialog_NewWindow":
                    return "新窗口";
                case "Documents_InsertHyperlinkDialog_SameFrame":
                    return "相同框架";
                case "Documents_InsertHyperlinkDialog_LinkTo":
                    return "链接到";
                case "Documents_InsertHyperlinkDialog_ExistingFrameOrWebpage":
                    return "原有文件或网页";
                case "Documents_InsertHyperlinkDialog_PlaceInDocument":
                    return "本文档中的位置";
                case "Documents_InsertHyperlinkDialog_Address":
                    return "地址";
                case "Documents_InsertHyperlinkDialog_SelectBookmark":
                    return "选择书签";
            }
            return base.GetStringOverride(key);
        }
    }
}
