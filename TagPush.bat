@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

cd /d "%~dp0"

set "CHANGELOG_FILE=CHANGELOG.md"
set "REMOTE=origin"

echo ========================================================
echo              Xiting.Atomic 打 tag 并推送脚本
echo ========================================================

rem 1. 检查当前目录是否为 Git 仓库
echo.
echo [1/7] 检查 Git 仓库...
git rev-parse --git-dir >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] 当前目录不是 Git 仓库
    goto :end
)
echo 当前目录是 Git 仓库.

rem 2. 检查工作区是否干净 (含未跟踪文件, 发布前源码必须完整)
echo.
echo [2/7] 检查工作区状态...
set "DIRTY="
for /f "delims=" %%a in ('git status --porcelain') do set "DIRTY=1"
if defined DIRTY (
    echo [ERROR] 工作区不干净, 存在未提交的修改或未跟踪文件, 请先提交并清理:
    echo.
    git status --short
    goto :end
)
echo 工作区干净.

rem 3. 从 CHANGELOG.md 提取最新版本条目 (形如 "## [1.0.0] - 2026-08-25", 排除 [Unreleased])
echo.
echo [3/7] 从 CHANGELOG 提取最新版本号...
set "VER="
for /f "tokens=2 delims=[]" %%a in ('findstr /r /c:"^## \[[0-9]" "%CHANGELOG_FILE%"') do (
    if not defined VER set "VER=%%a"
)
if not defined VER (
    echo [ERROR] %CHANGELOG_FILE% 中未找到已发布版本条目, 请先更新 CHANGELOG.md
    goto :end
)

rem 校验版本号格式 (x.y.z)
echo %VER%|findstr /r "^[0-9][0-9]*[.][0-9][0-9]*[.][0-9][0-9]*$" >nul 2>&1
if errorlevel 1 (
    echo [ERROR] CHANGELOG 中的版本号格式不正确, 当前值: [%VER%]
    goto :end
)

set "TAG=v%VER%"
echo 从 CHANGELOG 提取版本号: %VER%
echo 目标 tag: %TAG%

rem 4. 检查 tag 是否已存在
echo.
echo [4/7] 检查 tag 是否已存在...
set "EXISTING="
for /f "delims=" %%t in ('git tag -l "%TAG%"') do set "EXISTING=%%t"
if defined EXISTING (
    echo [ERROR] Tag %TAG% 已存在, 可能已经发布过, 请检查后手动处理
    goto :end
)
echo Tag %TAG% 不存在, 可以创建.

rem 5. 在创建 tag 前构建并测试
echo.
echo [5/7] 构建并测试...
dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo [ERROR] 构建失败, 请修复后重试
    goto :end
)
dotnet test --solution Atomic.slnx --configuration Release --no-build --verbosity normal
if %errorlevel% neq 0 (
    echo [ERROR] 测试失败, 请修复后重试
    goto :end
)
echo 构建与测试全部通过.

rem 6. 创建 tag
echo.
echo [6/7] 创建 tag 并推送...
git tag "%TAG%"
if %errorlevel% neq 0 (
    echo [ERROR] 创建 tag %TAG% 失败
    goto :end
)
echo Tag %TAG% 已创建.

rem 7. 推送 tag 到远程仓库
echo.
echo [7/7] 推送 tag 到远程仓库 %REMOTE%...
git push %REMOTE% "%TAG%"
if %errorlevel% neq 0 (
    echo [ERROR] 推送 tag %TAG% 失败, tag 已在本机创建, 可手动执行: git push %REMOTE% %TAG%
    goto :end
)
echo Tag %TAG% 已推送到 %REMOTE%.

echo.
echo ========================================================
echo       全部完成! GitHub Actions 将自动构建并发布
echo ========================================================

:end
echo.
pause
exit /b 0
