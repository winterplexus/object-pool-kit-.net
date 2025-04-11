Object Pool Applications for .NET
=================================

Application tool kit based on .NET 9 platform for implementing object pools using intrinsic concurrency and custom object pool classes.

## Kit Components

| Components                  | Description                                                             |
| :---------------------------|:------------------------------------------------------------------------|
| Object pool library         | Object pool container, member, manager classes including custom objects |
| Object pool kit application | Application to demonstrate how to implement object pools                |
| Object pool log library     | Log library using NLog logging platform classes                         |
| Object pool utility library | Utility classes to support above components                             |

## Kit Demonstration Options

```
usage: ObjectPool.Kit.exe (options)

options: -s <number of simulations>
         -t <number of parallel loops>
         -w <wait time between simulations>
         -l <object lifetime (seconds)>
         -p <object pool size>
         -u <object usage limit>
```