# CLAUDE.md

## 语言偏好

使用中文与用户交流。代码和注释可保留英文，但对话、解释和说明使用中文。

## Git 提交工作流

在提交和推送代码之前，按以下顺序执行：

1. `csharpier format .` — 格式化代码
2. `dotnet build` — 编译项目确保无错误
3. 执行 git 操作（add、commit、push）
