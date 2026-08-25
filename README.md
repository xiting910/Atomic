# Atomic

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![CI](https://github.com/xiting910/Atomic/actions/workflows/ci.yml/badge.svg)](https://github.com/xiting910/Atomic/actions/workflows/ci.yml)
[![CodeQL](https://github.com/xiting910/Atomic/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/xiting910/Atomic/actions/workflows/codeql-analysis.yml)
[![Dependency Review](https://github.com/xiting910/Atomic/actions/workflows/dependency-review.yml/badge.svg)](https://github.com/xiting910/Atomic/actions/workflows/dependency-review.yml)

---

## 🏗️ 项目结构

```
Atomic/
├── .github/                                        #   GitHub 配置
│   ├── ISSUE_TEMPLATE/                             #     Issue 模板
│   │   ├── bug_report.md                           #       Bug 报告模板
│   │   ├── config.yml                              #       Issue 模板选择配置 (文档链接)
│   │   └── feature_request.md                      #       功能建议模板
│   └── workflows/                                  #     GitHub Actions 工作流
│   │   ├── ci.yml                                  #       CI (构建/测试/上传测试结果)
│   │   ├── codeql-analysis.yml                     #       CodeQL 安全分析 (push/PR/每周定时)
│   │   ├── dependabot-auto-merge.yml               #       Dependabot PR 自动 approve + squash 合并
│   │   └── dependency-review.yml                   #       依赖漏洞审查 (PR 评论区报告)
│   ├── dependabot.yml                              #     Dependabot 依赖更新 (NuGet + Actions 分组策略)
│   └── PULL_REQUEST_TEMPLATE.md                    #     PR 描述模板
├── Xiting.Atomic/                                  #   原子操作库项目
│   ├── AtomicEnum.cs                               #     枚举原子操作封装结构体
│   ├── AtomicOperation.cs                          #     原子操作静态类 (SpinPredicateAndSet)
│   ├── AtomicOperationResult.cs                    #     原子操作结果枚举
│   └── Xiting.Atomic.csproj                        #     项目文件
├── Xiting.Atomic.Tests/                            #   原子操作库单元测试项目
│   ├── AtomicEnumTests.cs                          #     AtomicEnum 单元测试
│   ├── AtomicEnumConcurrencyTests.cs               #     AtomicEnum 并发测试
│   ├── AtomicOperationPrimitiveTests.cs            #     primitive 类型重载单元测试
│   ├── AtomicOperationGenericTests.cs              #     泛型重载单元测试
│   ├── AtomicOperationConcurrencyTests.cs          #     AtomicOperation 并发测试
│   ├── Test.cs                                     #     测试辅助类型 (Test/FlagsTest/UnsupportedStruct)
│   └── Xiting.Atomic.Tests.csproj                  #     项目文件
├── .editorconfig                                   #   代码风格统一配置
├── .gitattributes                                  #   Git 行尾归一化 (默认 LF), diff 策略与二进制标记
├── .gitignore                                      #   忽略规则
├── CHANGELOG.md                                    #   变更日志
├── Directory.Build.props                           #   全局构建属性 (TargetFramework / Nullable / CPM)
├── Directory.Packages.props                        #   集中包版本管理 (NuGet CPM)
├── global.json                                     #   测试平台运行器配置 (Microsoft.Testing.Platform)
├── LICENSE                                         #   MIT 许可证
├── README.md                                       #   本文档
└── Atomic.slnx                                     #   解决方案文件 (.NET XML 格式)
```

---

## 🚀 快速开始

### 环境要求

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### 克隆

```bash
git clone https://github.com/xiting910/Atomic.git
cd Atomic
```

### 构建

```bash
dotnet build
```

### 运行测试

```bash
dotnet test
```

---

## 📄 许可证

本项目采用 [MIT License](LICENSE)。
