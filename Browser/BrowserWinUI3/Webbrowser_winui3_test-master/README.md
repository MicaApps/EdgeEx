# Webbrowser_winui3_UI

## 需求列表


| 位置 | 功能 | 当前样式 | 目标设计图样式 |
| ------------- | ------------- |------------- |------------- |
| 1.书签管理器 | 导入，导出，读取，| 
|  | 展示为侧栏（文件夹）-列表项 | | ![image](https://user-images.githubusercontent.com/6630660/230351458-21dd3346-476d-4798-bf5d-c2fa0649f72d.png) |
|  | 展示为侧栏（文件夹）-缩略图 | | ![image](https://user-images.githubusercontent.com/6630660/230351582-3c63f527-e606-40d7-9783-4251bff63f80.png) |
|  | 展示为侧栏（文件夹）-带缩略图的列表项 | | ![image](https://user-images.githubusercontent.com/6630660/230351690-cc201146-90f1-4b5c-aa28-95ae6ff329f1.png) |
|  | 从当前页面新建书签（未知）（等价于收藏当前页面） |
| 2.下载管理器| 展示当前正在进行的下载和最近的下载 |  | ![image](https://user-images.githubusercontent.com/6630660/230349493-07459e3c-c8b3-42dd-99e2-09c026ceb291.png) |
|  | 在文件夹中打开下载项 |
|  | 暂停、继续、取消、重新开始下载 |
|  | 获取和展示下载进度 |
| 3.历史记录| UI对齐到设计图 | ![image](https://user-images.githubusercontent.com/6630660/230349278-b37e5ff6-8b77-487d-8eee-38c562975e27.png) | ![image](https://user-images.githubusercontent.com/6630660/230348799-dc039034-561b-43bf-a5cf-839d3530e07b.png)
|  | 展示为列表项 |   | ![image](https://user-images.githubusercontent.com/6630660/230348799-dc039034-561b-43bf-a5cf-839d3530e07b.png)
|  | 展示为缩略图 |
|  | 展示为带缩略图的列表项 |
| 4.标签项| UI对齐到设计图 | ![image](https://user-images.githubusercontent.com/6630660/230350084-9048f880-32bf-4e08-aadd-9f5c495d5f02.png) |![52512b7f71ba33c768fa647be7182efa](https://user-images.githubusercontent.com/6630660/230349960-519d56ad-b4ae-4f19-b868-ad2c57fa21fe.png) |
| 5.新标签页 |移除搜索框|
| 6.多功能框（地址和搜索栏） |UI对齐到设计图| ![1d647a3dbac0fca5c48ec984189781ad](https://user-images.githubusercontent.com/6630660/230350361-662066f3-8a27-4400-99e3-890609ee4fc6.png) | ![b3c06bbbc3dc5c8f7a8b86725d64b12d](https://user-images.githubusercontent.com/6630660/230350568-475a2b6d-58e0-456b-acab-c97afe8a9c1f.png) |
|  |在中文输入法输入候选词回车后应保留英文继续输入，不要直接跳转| 
| 7.多功能框建议列表 |搜索建议（从当前打开的标签页）|
|  | 搜索建议（来自搜索引擎） |
|  | 搜索建议（来自历史记录） |
| 8.前进后退按钮 |功能失效|
| 9.背景 | 亚克力背景对齐到设计图(应当近似于UWP中的{themeresources systemdefaultwindowbrush}) |
|  | 新增 Mica 背景 |
