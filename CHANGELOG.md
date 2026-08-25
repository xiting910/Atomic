# Changelog

本文件记录了项目的所有重要变更。每个版本的变更都应在发布时记录在此文件中。

格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.1.0/),
版本号遵循 [Semantic Versioning](https://semver.org/lang/zh-CN/).

---

## [Unreleased]

### Added

- 初始提交, 创建 Xiting.Atomic 原子操作库项目, 包含:
  - <code>AtomicOperation</code> 静态类: 自旋模式的 <code>SpinPredicateAndSet</code> 原子操作, 支持
    byte/sbyte/short/ushort/int/uint/long/ulong 及泛型(引用类型、primitive、枚举)重载
  - <code>AtomicEnum&lt;TEnum&gt;</code> 结构体: 原子读写、CompareAndSet、PredicateAndSet、SpinPredicateAndSet 操作
  - <code>AtomicOperationResult</code> 枚举: 表示原子操作结果
  - 单元测试与并发测试项目 (xunit.v3)
  - 项目基础设施: CI/CodeQL/Dependabot 工作流、Issue/PR 模板、.editorconfig、.gitattributes、
    .gitignore、MIT 许可证、README 与 CHANGELOG

[Unreleased]: https://github.com/xiting910/Atomic/commits/main
