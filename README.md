# 经典仓储

> 阶段性技术总结汇总
## 基本

采用`.net core 5.0`构建，ORM使用`FreeSql`

## 开发中IIS支持配置方法
1. 运行 `Visual Studio Installer` 安装程序，修改已安装的组件，在`NET Core 跨平台开发` 右侧栏中勾选 `开发时间 IIS 支持`

2. 编辑系统的hosts文件，路径在`C:\Windows\System32\drivers\etc\hosts`，结尾增加一行：
```bash
    127.0.0.1       rep.litdev.me
```

3. IIS中新建网站，网站名称任意，路径指到项目中的Web目录，绑定域名直接填写host中配置的 `rep.litdev.me`

4. 右键Web项目-属性，点击左侧`调试`栏，配置文件-`新建`，`启动`栏中选择`IIS`，`环境变量`中添加Key：`ASPNETCORE_ENVIRONMENT`,Value：`Development`，`Web服务器设置`-`应用URL`填写：`http://rep.litdev.me`，下面的保持默认就行了

5. 这种模式需要Visual Studio以管理员身份运行，强制Visual Studio默认以管理员身份运行，[具体步骤参见](https://www.cnblogs.com/tanfuchao/p/8978595.html)