# Changelog

本文件记录了项目的所有重要变更。每个版本的变更都应在发布时记录在此文件中。

格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.1.0/),
版本号遵循 [Semantic Versioning](https://semver.org/lang/zh-CN/).

---

## [Unreleased]

### Added

- 新增 <code>TagPush.bat</code> 发布脚本: 检查工作区干净状态, 从 <code>CHANGELOG.md</code> 提取最新版本号,
  构建测试通过后创建并推送 <code>v*</code> tag, 触发 GitHub Actions 自动发布
- <code>README.md</code> 添加 NuGet 下载量徽章

### Changed

- <code>.github/dependabot.yml</code> 移除 NuGet 依赖的分组合并更新 (xunit/test-sdk/catchall 三组), 恢复为每个依赖单独 PR
- <code>README.md</code> 更新: 添加 NuGet 徽章、项目简介、特性列表与安装章节, 项目结构补充 <code>TagPush.bat</code>
- <code>.gitignore</code> 优化 TestResults 忽略规则: 从 <code>TestResults/*</code> 改为 <code>TestResults/</code>
- <code>Xiting.Atomic.csproj</code> 添加 <code>PackageValidationBaselineVersion</code> (1.0.0) 包验证基线,
  与 v1.0.0 对比防止意外破坏公共 API

---

## [1.0.0] - 2026-08-25

### Added

- 初始提交, 创建 Xiting.Atomic 原子操作库项目, 包含:
  - <code>AtomicOperation</code> 静态类: 自旋模式的 <code>SpinPredicateAndSet</code> 原子操作, 支持
    byte/sbyte/short/ushort/int/uint/long/ulong 及泛型(引用类型、primitive、枚举)重载
  - <code>AtomicEnum&lt;TEnum&gt;</code> 结构体: 原子读写、CompareAndSet、PredicateAndSet、SpinPredicateAndSet 操作
  - <code>AtomicOperationResult</code> 枚举: 表示原子操作结果
  - 单元测试与并发测试项目 (xunit.v3)
  - 项目基础设施: CI/CodeQL/Dependabot 工作流、Issue/PR 模板、.editorconfig、.gitattributes、
    .gitignore、MIT 许可证、README 与 CHANGELOG
- 添加 NuGet 打包发布支持:
  - <code>Xiting.Atomic.csproj</code> 打包元数据: PackageId/Authors/Description/MIT 许可证/仓库地址/
    标签/包内 README/符号包
  - <code>release-publish.yml</code> 发布工作流: 推 <code>v*</code> tag 自动测试、打包,
    通过 Trusted Publishing (OIDC) 发布 NuGet 包并创建 GitHub Release
  - 包内 README (<code>Xiting.Atomic/README.md</code>), 供 nuget.org 包页面展示
  - <code>PublicAPI.Shipped.txt</code>/<code>PublicAPI.Unshipped.txt</code> 固定公共 API 契约
  - <code>Microsoft.CodeAnalysis.PublicApiAnalyzers</code> 与 <code>Microsoft.SourceLink.GitHub</code>
  - <code>EnablePackageValidation</code> 包验证与 <code>Deterministic</code> 确定性构建

[Unreleased]: https://github.com/xiting910/Atomic/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/xiting910/Atomic/releases/tag/v1.0.0
