# Xiting.Atomic

一个轻量级的无锁原子操作库, 提供针对枚举类型和 primitive 类型的原子读写与基于自旋的比较-交换 (CAS) 操作, 适用于多线程并发场景。

## 功能特性

- 原子读写: 通过 `AtomicEnum<TEnum>` 封装, 提供原子化的枚举值读写
- 比较并设置: `CompareAndSet` 只在当前值等于期望值时原子更新
- 谓词并设置: `PredicateAndSet` 只在当前值满足条件时原子更新
- 自旋重试: `SpinPredicateAndSet` 在发生竞态时自动自旋重试, 直至成功或确定失败
- 支持类型: byte/sbyte/short/ushort/int/uint/long/ulong 以及引用类型、primitive、枚举的泛型操作
- 无锁实现: 基于 `Interlocked` 与 `Volatile`, 不依赖 `lock`

## 安装

```bash
dotnet add package Xiting.Atomic
```

要求 .NET 10.0 及以上。

## 快速开始

### 原子枚举

```csharp
using Xiting.Atomic;

enum ConnectionState { Disconnected, Connecting, Connected }

var state = new AtomicEnum<ConnectionState>(ConnectionState.Disconnected);

// 原子读取
var current = state.Value;

// 仅在当前值为 Disconnected 时更新为 Connecting
var result = state.PredicateAndSet(
    s => s == ConnectionState.Disconnected,
    ConnectionState.Connecting,
    out var originalValue);
// result == AtomicOperationResult.Success, originalValue == Disconnected

// 自旋版本: 发生竞态时自动重试, 直至成功或条件确定失败
var changed = state.SpinPredicateAndSet(
    s => s == ConnectionState.Connecting,
    s => ConnectionState.Connected,
    out originalValue,
    out var finalValue);
```

### 原子 primitive

```csharp
using Xiting.Atomic;

int state = 0;

// 仅在当前值为 0 时原子更新为 1
bool changed = AtomicOperation.SpinPredicateAndSet(ref state, s => s == 0, 1);

// 使用工厂方法生成新值, 并发竞争时自动自旋重试
AtomicOperation.SpinPredicateAndSet(ref state, s => s < 10, s => s + 1);

// 泛型版本: 支持引用类型、primitive 和枚举
string? value = null;
bool initialized = AtomicOperation.SpinPredicateAndSet(ref value, v => v is null, "initialized");
```

## API 参考

### AtomicOperation (静态类)

| 方法                                                                                     | 说明                                       |
| ---------------------------------------------------------------------------------------- | ------------------------------------------ |
| `SpinPredicateAndSet(ref byte/sbyte/short/ushort/int/uint/long/ulong, Func<T, bool>, T)` | 自旋: 当前值满足条件且不等于新值时原子更新 |
| `SpinPredicateAndSet(ref T, Func<T, bool>, Func<T, T>)`                                  | 自旋版本, 新值由工厂方法根据当前值生成     |
| `SpinPredicateAndSet<T>(ref T, Func<T, bool>, T)`                                        | 泛型版本, T 为引用类型/primitive/枚举      |
| `SpinPredicateAndSet<T>(ref T, Func<T, bool>, Func<T, T>)`                               | 泛型 + 工厂版本                            |

所有重载返回 `bool`: 值发生变化返回 `true`, 条件不满足或值已相同返回 `false`。

### AtomicEnum\<TEnum\> (结构体, where TEnum : struct, Enum)

| 成员                                     | 说明                                                    |
| ---------------------------------------- | ------------------------------------------------------- |
| `AtomicEnum(TEnum initialValue)`         | 构造, 指定初始值                                        |
| `Value`                                  | 原子读写当前枚举值                                      |
| `Set(TEnum, out TEnum)`                  | 原子替换, 返回是否发生变化                              |
| `CompareAndSet(TEnum, TEnum, out TEnum)` | 当前值等于比较值时才更新                                |
| `PredicateAndSet(...)` ×2                | 当前值满足谓词时更新, 竞态时返回 `RaceCondition` 不重试 |
| `SpinPredicateAndSet(...)` ×2            | 自旋版本, 竞态时自动重试                                |
| `==` / `!=`                              | 与 `TEnum` 及 `AtomicEnum<TEnum>` 相互比较              |
| 隐式转换                                 | `AtomicEnum<TEnum>` 与 `TEnum` 互转                     |

### AtomicOperationResult (枚举)

| 值                | 说明                       |
| ----------------- | -------------------------- |
| `AlreadySet`      | 已经是指定的值, 操作未执行 |
| `PredicateFailed` | 谓词条件不满足, 操作未执行 |
| `RaceCondition`   | 发生竞态条件, 操作未成功   |
| `Success`         | 操作成功, 值已更新         |

## 注意事项

- `AtomicEnum.Value` 的读写是原子的, 但读取-修改-写入的复合操作必须使用 `CompareAndSet` 或
  `PredicateAndSet` 系列方法, 否则无法保证原子性。
- `PredicateAndSet` 在发生竞态时返回 `RaceCondition` 且不会自动重试, 需要由调用方决定如何处理;
  若期望自动重试, 请使用 `SpinPredicateAndSet`。
- 泛型 `SpinPredicateAndSet<T>` 仅支持引用类型、primitive 和枚举, 其他结构体会抛出
  `NotSupportedException`; 枚举类型建议优先使用 `AtomicEnum<TEnum>`。
- `predicate` 与 `newValueFactory` 参数为 `null` 时抛出 `ArgumentNullException`。
- 自旋操作适合临界区很短的场景, 请勿在自旋条件内执行耗时操作。

### AtomicEnum\<TEnum\> 结构体使用注意事项

`AtomicEnum<TEnum>` 是结构体, 使用时需要注意:

- 不推荐作为方法参数传递: 结构体按值传递会创建副本, 副本间相互独立, 对副本的原子操作不会作用到原值
- 不推荐作为属性暴露: 属性 getter 返回的是值的副本, 获取后与内部值不再同步
- 作为字段时不要添加 `readonly`: 对 `readonly` 字段的成员访问会产生防御性副本, 修改会作用在副本上导致原子操作失效
- 所有作为结构体需要注意的情况, `AtomicEnum<TEnum>` 同样需要注意

## 许可证

MIT License, 详见 [LICENSE](https://github.com/xiting910/Atomic/blob/main/LICENSE)。
