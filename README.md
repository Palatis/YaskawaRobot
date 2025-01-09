# Yaskawa.Robot
Yaskawa High Speed Ethernet Server client for .NET

This project is derived from [df910105/YaskawaRobot](https://github.com/df910105/YaskawaRobot).

## Support Matrix
| Robot Control Function                                     | R/W | YRC1000 | YRC1000micro | DX100/DX200 | FS100 |
|------------------------------------------------------------|-----|---------|--------------|-------------|-------|
| Alarm Data                                                 | R   | v       | v            | v           | v     |
| Alarm History                                              | R   | v       | v            | v           | v     |
| Status Information                                         | R   | v       | v            | v           | v     |
| Executing Job Information                                  | R   | v       | v            | v           | v     |
| Axis Config Information                                    | R   | v       | v            | v           | v     |
| Robot Position Data                                        | R   | v       | v            | v           | v     |
| Position Error                                             | R   | v       | v            | v           | v     |
| Torque Data                                                | R   | v       | v            | v           | v     |
| I/O Data                                                   | RW  | v       | v            | v           | v     |
| Register Data                                              | RW  | v       | v            | v           | v     |
| Byte Variable (B)                                          | RW  | v       | v            | v           | v     |
| Integer Type Variable (I)                                  | RW  | v       | v            | v           | v     |
| Double Precision Integer Type Variable (D)                 | RW  | v       | v            | v           | v     |
| Real Type Variable (R)                                     | RW  | v       | v            | v           | v     |
| String Type Variable (S)                                   | RW  | v       | v            | v           | v     |
| Robot Position Type Variable (P)                           | RW  | v       | v            | v           | v     |
| Base Position Type Variable (BP)                           | RW  | v       | v            | v           | v     |
| External Axis Type Variable (EX)                           | RW  | v       | v            | v           | v     |
| Alarm Reset / Error Cancel                                 | W   | v       | v            | v           | v     |
| Hold / Servo On/off                                        | W   | v       | v            | v           | v     |
| Step / Cycle / Auto Switching                              | W   | v       | v            | v           | v     |
| Character String Display To The Programming Pendant        | W   | v       | v            | v           | v     |
| Start-up (Job Start)                                       | W   | v       | v            | v           | v     |
| Job Select                                                 | W   | v       | v            | v           | v     |
| Management Time                                            | R   | v       | v            | v           | v     |
| System Information                                         | R   | v       | v            | v           | v     |
| Plural I/O Data                                            | RW  | ?       | ?            | ?           | ?     |
| Plural Register Data                                       | RW  | ?       | ?            | ?           | ?     |
| Plural Byte Type Variable (B)                              | RW  | ?       | ?            | ?           | ?     |
| Plural Integer Type Variable (I)                           | RW  | ?       | ?            | ?           | ?     |
| Plural Double Precision Integer Type Variable (D)          | RW  | ?       | ?            | ?           | ?     |
| Plural Real Type Variable (R)                              | RW  | ?       | ?            | ?           | ?     |
| Plural String Type Variable (S)                            | RW  | ?       | ?            | ?           | ?     |
| Plural Robot Position Type Variable (P)                    | RW  | ?       | ?            | ?           | ?     |
| Plural Base Position Type Variable (BP)                    | RW  | ?       | ?            | ?           | ?     |
| Plural Station Type Variable (EX)                          | RW  | ?       | ?            | ?           | ?     |
| Alarm Data (for Applying the Sub Code Character String)    | RW  | ?       | ?            | ?           | ?     |
| Alarm History (for Applying the Sub Code Character String) | RW  | ?       | ?            | ?           | ?     |
| Move instruction (Type Cartesian Coordinate)               | W   | v       | v            | v           | v     |
| Move Instruction (Type Pulse)                              | W   | v       | v            | v           | v     |
| 32 Byte Character Type Variable (S)                        | RW  | ?       | ?            | x           | x     |
| Plural 32 Byte Character Type Variable (S)                 | RW  | ?       | ?            | x           | x     |
| Encoder Temperature                                        | R   | ?       | ?            | x           | x     |
| Amount of Regenerative Power                               | R   | ?       | x            | x           | x     |
| Converter Temperature                                      | R   | ?       | ?            | x           | x     |

| File Function       | YRC1000 | YRC1000micro | DX100/DX200 | FS100 |
|---------------------|---------|--------------|-------------|-------|
| File Deleting       | x       | x            | x           | x     |
| File Loading        | x       | x            | x           | x     |
| File Saving         | x       | x            | x           | x     |
| File List Acquiring | x       | x            | x           | x     |
| Batch Data Backup   | x       | x            | x           | x     |

- o: implemented
- ?: supported, not implemented
- x: not supported

## Usage
```C#
using Yaskawa.Robot.EthernetServer.UDP;

class Program
{
    static void Main()
    {
        MotoComHS yrc1000 = new MotoComHS("192.168.255.1");
        
        SystemInfo systemInfo = new SystemInfo();
        rt = yrc1000.ReadSystemInfoData(11, ref systemInfo, out err);
        Console.WriteLine(systemInfo);
    }
}
```

## References
* https://github.com/hsinkoyu/fs100
* https://github.com/df910105/YaskawaRobot

## Documentations
* [FS100 Options Instructions for High-speed Ethernet Server Function](https://www.motoman.com/getmedia/16B5CD92-BD0B-4DE0-9DC9-B71D0B6FE264/160766-1CD.pdf.aspx?ext=.pdf)
* [DX100 Options Instructions for High-speed Ethernet Server Function](https://www.motoman.com/getmedia/B0E02647-FF2F-45EF-97D8-41F59B92B07A/162529-1CD.pdf.aspx?ext=.pdf)
* [YRC1000 Options Instructions for Ethernet Function](https://www.motoman.com/getmedia/38CD89D5-C90D-4C5A-8628-0551C44C9A6C/178942-1CD.pdf.aspx?ext=.pdf)
* [YRC1000micro Options Instructions for Ethernet Function](https://www.motoman.com/getmedia/992F83D8-CF68-4D50-8F2C-EB1E940478ED/181259-1cd.pdf.aspx?ext=.pdf)

## Additional Resources
* [Remote Operation with Yaskawa Motoman Robots](https://knowledge.motoman.com/hc/en-us/articles/4408154769047-Remote-Operations-with-Yaskawa-Motoman-Robots)
* [NX100 Options Instructions for Ethernet Server Function](https://www.motoman.com/getmedia/9F9524FF-1EEE-4708-B8FB-24E1BAAC8247/153543-1cd.pdf.aspx?ext=.pdf): (different remote control protocol)
