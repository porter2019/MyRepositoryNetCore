# 经典仓储结构

> 阶段性技术总结汇总
## Basic

采用`.net core 5.0`构建，ORM使用`FreeSql`

## 依赖注入
IOC使用NetCore自带的，为了方便批量注入，现定义以下约定
1. 所有`非实体数据操作`的Services(`实体的Service在FreeSql中批量注入`)，需要在自定义的IService接口中继承`IBatchDIServicesTag`接口
2. Services实现类中，需要给Services类增加`[ServiceLifetime()]`自定义特性，指定参数`IsEnabled`为该服务是否启用，默认的Services生命周期是`Scoped`，如果需其它，则在`Lifetime`属性中指定
##### 拓展，注入了多个实现类的情况
直接在`构造函数`中或者在参数中通过`[FromServices]`获取的服务，默认返回的是`最后注入的那个实现类`，如果获取所有的实现类，则有以下两种方法
- 通过`IServiceProvider`
    ```
    public Task<ApiResult> InvokeAsync(HttpContext context, IServiceProvider provider)
    {
        foreach (var service in provider.GetServices<IDemoService>())
        {
            //TODO 拿到service
        }
        return Task.FromResult(ApiResult.OK());
    }
    ```
    > 这种方法接收`IServiceProvider`类型(容器)的注入，直接通过容器中的GetServices<<I>>方法获取容器中所有该接口的注入
- IEnumerable
    ```
    public Task<ApiResult> InvokeAsync(HttpContext context, IEnumerable<IDemoService> svs)
    {
        foreach (var service in svs)
        {
            //TODO 拿到service
        }
        return Task.FromResult(ApiResult.OK());
    }
    ```
    > 这种方法接收`IEnumerable<IDemoService>`类型的注入



## 实体需要标识的FreeSql特性说明
- 实体分页查询、GetModel需要查询sql视图的情况
    建立视图对应的dto类，一般继承实体，在dto增加需要查询额外的属性，给该dto增加FreeSql特性
    ```
    [FsTable("说明", Name = "BookInfoView", DisableSyncStructure = true)]
    ```
    > 必须指定`Name`，值为sql中视图的名称，并设置`DisableSyncStructure = true`禁用迁移

    继承的实体中的FreeSql特性如下：
    ```
    [FsTable("表备注", ViewClassName = typeof(DTO文件类名), VueModuleName = "demo")]
    ```
    > 必须指定`ViewClassName`，名称为dto的类名，`VueModuleName`为前端生成Vue代码的模块名称，生成的pages对应的目录就是`/views/demo/实体名称/index.vue`，同时`router`中也对自动设置好访问的路由地址
- 有明细操作的实体
    指定主实体的FreeSql特性
    ```
    [FsTable("演示主体", HaveItems = true, VueModuleName = "demo")]
    ```
    > 必须设置`HaveItems=true`，用于前端代码生成自动生成明细表演示代码

## 代码生成器使用说明
![接口列表](/doc/md/swagger.jpg)
#### 1.<a name="code-generate-api-controller">生成api控制器</a>
参数：name(实体名)，desc(实体说明)
A.如果name是表的实体名称，则desc参数留空，说明会自动从属性中取得
B.如果name不是表的实体，则生成一个基础的api控制器模板，需要填写desc
#### 2.生成实体IRepository、IServices、Repository、Services四个层的代码文件
生成实体的仓储层和服务层代码，同时会在`项目名称.Model.RequestModel`目录下生成分页接口请求所需的dto
#### 3. 生成前端Vue页面代码，包括api、route、pages
调这个接口前需保证在`appsettings.json`中配置了Vue项目的根目录
```
"VueProjectDirectory": "C:\\WorkSpace\\GitHub\\MyNetCore-Web",
```
参数说明同<a href="#code-generate-api-controller">第一个接口</a>，如果非实体，则生成基础模板
- ##### api.js
    生成基础四个方法，有额外的方法对应添加
- ##### route
    路由文件生成到这个目录`/router/modules/实体名称.js`,每一个实体都会生成一个这种配置，如果需要将该实体的index、edit、show操作放到别的模块下，则需要手动复制文件`children`中的代码到相应的路由配置下
- ##### pages
    ###### index.vue页面还需要根据业务调整的地方如下：
    - table的item对应加载了实体的列，可能还需要根据具体属性特性调整列长度、format的格式、列删减等操作
    - 搜索的字段，目前默认只显示了一个时间范围的查询，如果需要查询其它列，则自定义追加，别忘了自动生成的请求分页实体也需要加上对应的查询列
    - `sortChange`方法，添加排序字段映射`columnMap.set("SexText", "Sex");`

    ###### edit.vue页面还需要根据业务调整的地方如下：
    - 如果是`富文本(实体指定DbType="text")`或者`Remark`属性，则需要放在单独的`el-row`行中，并设置`el-col` `:span="20"`
    - 如果是上传图片，则需要手动在`el-col`中设置input

    ```
        //template
       <el-col :span="20">
            <el-form-item label="图片">
                <image-upload :url.sync="formData.ImagePathFull" :data="{'tag':'images'}" @on-success="imageUploadSuccess" />
            </el-form-item>
        </el-col>

        //methods
        imageUploadSuccess(res) {
            if (res.code === 200) {
                if ((res.data || []).length > 0) {
                    this.formData.ImagePathFull = res.data[0].FileWebPath;
                    this.formData.ImagePath = res.data[0].FilePath;
                } else {
                    this.$message.error("上传成功，但是没有返回上传后的文件");
                }
            } else {
                this.$message.error(res.msg);
            }
        },

    ```
    > `tag`属性为文件最终保存的目录，不指定这个参数则默认`attach`
    - 如果有明细，明细中的列代码并未自动生成，只写了一些演示的数据，对应的做调整就行

    ###### show.vue页面还需要根据业务调整的地方如下：
    - 如果是`富文本(实体指定DbType="text")`或者`Remark`属性，则需要放在单独的`el-row`行中，并设置`el-col` `:span="20"`
    - 如果是图片属性，则需要手动在`el-col`中设置

    ```
        <el-col :span="20">
            <el-form-item label="图片">
                <image-preview v-if="formData.ImagePathFull" :src="formData.ImagePathFull" :width="100" />
            </el-form-item>
        </el-col>
    ```
    - 如果有明细，明细中的列代码并未自动生成，只写了一些演示的数据，对应的做调整就行

## 缓存
定义了统一的缓存服务接口`ICacheServices`，并实现了`Redis`和`MemoryCache`两种方式，配置文件中如果启用了Redis，则业务中的缓存自动使用Redis，否则使用MemoryCache

## 二维码
使用[QRCoder](https://github.com/codebude/QRCoder)组件

## 图形验证码
依赖`System.Drawing.Common`库

## 开发中IIS支持配置方法
1. 运行 `Visual Studio Installer` 安装程序，修改已安装的组件，在`NET Core 跨平台开发` 右侧栏中勾选 `开发时间 IIS 支持`

2. 编辑系统的hosts文件，路径在`C:\Windows\System32\drivers\etc\hosts`，结尾增加一行：
```bash
127.0.0.1       rep.litdev.me
```

3. IIS中新建网站，网站名称任意，路径指到项目中的Web目录，绑定域名直接填写host中配置的 `rep.litdev.me`

4. 右键Web项目-属性，点击左侧`调试`栏，配置文件-`新建`，`启动`栏中选择`IIS`，`环境变量`中添加Key：`ASPNETCORE_ENVIRONMENT`,Value：`Development`，`Web服务器设置`-`应用URL`填写：`http://rep.litdev.me`，下面的保持默认就行了

5. 这种模式需要Visual Studio以管理员身份运行，强制Visual Studio默认以管理员身份运行，[具体步骤参见](https://www.cnblogs.com/tanfuchao/p/8978595.html)