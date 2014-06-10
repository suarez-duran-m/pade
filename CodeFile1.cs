 // Take the selected adapter
 //           PacketDevice selectedDevice = allDevices[deviceIndex - 1];

 //           // Open the output device
 //           using (PacketCommunicator communicator = selectedDevice.Open(100, // name of the device
 //                                                                        PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
 //                                                                        1000)) // read timeout
 //           {
 //               // Supposing to be on ethernet, set mac source to 01:01:01:01:01:01
 //               MacAddress source = new MacAddress("01:01:01:01:01:01");

 //               // set mac destination to 02:02:02:02:02:02
 //               MacAddress destination = new MacAddress("02:02:02:02:02:02");

 //               // Create the packets layers

 //               // Ethernet Layer
 //               EthernetLayer ethernetLayer = new EthernetLayer
 //                                                 {
 //                                                     Source = source,
 //                                                     Destination = destination
 //                                                 };

 //               // IPv4 Layer
 //               IpV4Layer ipV4Layer = new IpV4Layer
 //                                         {
 //                                             Source = new IpV4Address("1.2.3.4"),
 //                                             Ttl = 128,
 //                                             // The rest of the important parameters will be set for each packet
 //                                         };

 //               // ICMP Layer
 //               IcmpEchoLayer icmpLayer = new IcmpEchoLayer();

 //               // Create the builder that will build our packets
 //               PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, icmpLayer);

 //               // Send 100 Pings to different destination with different parameters
 //               for (int i = 0; i != 100; ++i)
 //               {
 //                   // Set IPv4 parameters
 //                   ipV4Layer.CurrentDestination = new IpV4Address("2.3.4." + i);
 //                   ipV4Layer.Identification = (ushort)i;

 //                   // Set ICMP parameters
 //                   icmpLayer.SequenceNumber = (ushort)i;
 //                   icmpLayer.Identifier = (ushort)i;

 //                   // Build the packet
 //                   Packet packet = builder.Build(DateTime.Now);

 //                   // Send down the packet
 //                   communicator.SendPacket(packet);
 //               }

 //               communicator.SendPacket(BuildEthernetPacket());
 //               communicator.SendPacket(BuildArpPacket());
 //               communicator.SendPacket(BuildVLanTaggedFramePacket());
 //               communicator.SendPacket(BuildIpV4Packet());
 //               communicator.SendPacket(BuildIcmpPacket());
 //               communicator.SendPacket(BuildIgmpPacket());
 //               communicator.SendPacket(BuildGrePacket());
 //               communicator.SendPacket(BuildUdpPacket());
 //               communicator.SendPacket(BuildTcpPacket());
 //               communicator.SendPacket(BuildDnsPacket());
 //               communicator.SendPacket(BuildHttpPacket());
 //               communicator.SendPacket(BuildComplexPacket());
 //           }
 //       }

 //       /// <summary>
 //       /// This function build an Ethernet with payload packet.
 //       /// </summary>
 //       private static Packet BuildEthernetPacket()
 //       {
 //           EthernetLayer ethernetLayer =
 //               new EthernetLayer
 //                   {
 //                       Source = new MacAddress("01:01:01:01:01:01"),
 //                       Destination = new MacAddress("02:02:02:02:02:02"),
 //                       EtherType = EthernetType.IpV4,
 //                   };

 //           PayloadLayer payloadLayer =
 //               new PayloadLayer
 //                   {
 //                       Data = new Datagram(Encoding.ASCII.GetBytes("hello world")),
 //                   };

 //           PacketBuilder builder = new PacketBuilder(ethernetLayer, payloadLayer);

 //           return builder.Build(DateTime.Now);
 //       }

 //       /// <summary>
 //       /// This function build an ARP over Ethernet packet.
 //       /// </summary>
 //       private static Packet BuildArpPacket()
 //       {
 //           EthernetLayer ethernetLayer =
 //               new EthernetLayer
 //                   {
 //                       Source = new MacAddress("01:01:01:01:01:01"),
 //                       Destination = new MacAddress("02:02:02:02:02:02"),
 //                       EtherType = EthernetType.None, // Will be filled automatically.
 //                   };

 //           ArpLayer arpLayer =
 //               new ArpLayer
 //                   {
 //                       ProtocolType = EthernetType.IpV4,
 //                       Operation = ArpOperation.Request,
 //                       SenderHardwareAddress = new byte[] {3, 3, 3, 3, 3, 3}.AsReadOnly(), // 03:03:03:03:03:03.
 //                       SenderProtocolAddress = new byte[] {1, 2, 3, 4}.AsReadOnly(), // 1.2.3.4.
 //                       TargetHardwareAddress = new byte[] {4, 4, 4, 4, 4, 4}.AsReadOnly(), // 04:04:04:04:04:04.
 //                       TargetProtocolAddress = new byte[] {11, 22, 33, 44}.AsReadOnly(), // 11.22.33.44.
 //                   };

 //           PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);

 //           return builder.Build(DateTime.Now);
 //       }

 //       /// <summary>
 //       /// This function build a VLanTaggedFrame over Ethernet with payload packet.
 //       /// </summary>
 //       private static Packet BuildVLanTaggedFramePacket()
 //       {
 //           EthernetLayer ethernetLayer =
 //               new EthernetLayer
 //                   {
 //                       Source = new MacAddress("01:01:01:01:01:01"),
 //                       Destination = new MacAddress("02:02:02:02:02:02"),
 //                       EtherType = EthernetType.None, // Will be filled automatically.
 //                   };

 //           VLanTaggedFrameLayer vLanTaggedFrameLayer =
 //               new VLanTaggedFrameLayer
 //                   {
 //                       PriorityCodePoint = ClassOfService.Background,
 //                       CanonicalFormatIndicator = false,
 //                       VLanIdentifier = 50,
 //                       EtherType = EthernetType.IpV4,
 //                   };

 //           PayloadLayer payloadLayer =
 //               new PayloadLayer
 //                   {
 //                       Data = new Datagram(Encoding.ASCII.GetBytes("hello world")),
 //                   };

 //           PacketBuilder builder = new PacketBuilder(ethernetLayer, vLanTaggedFrameLayer, payloadLayer);

 //           return builder.Build(DateTime.Now);
 //       }

 //       /// <summary>
 //       /// This function build an IPv4 over Ethernet with payload packet.
 //       /// </summary>
 //       private static Packet BuildIpV4Packet()
 //       {
 //           EthernetLayer ethernetLayer =
 //               new EthernetLayer
 //                   {
 //                       Source = new MacAddress("01:01:01:01:01:01"),
 //                       Destination = new MacAddress("02:02:02:02:02:02"),
 //                       EtherType = EthernetType.None,
 //                   };

 //           IpV4Layer ipV4Layer =
 //               new IpV4Layer
 //                   {
 //                       Source = new IpV4Address("1.2.3.4"),
 //                       CurrentDestination = new IpV4Address("11.22.33.44"),
 //                       Fragmentation = IpV4Fragmentation.None,
 //                       HeaderChecksum = null, // Will be filled automatically.
 //                       Identification = 123,
 //                       Options = IpV4Options.None,
 //                       Protocol = IpV4Protocol.Udp,
 //                       Ttl = 100,
 //                       TypeOfService = 0,
 //                   };

 //           PayloadLayer payloadLayer =
 //               new PayloadLayer
 //                   {
 //                       Data = new Datagram(Encoding.ASCII.GetBytes("hello world")),
 //                   };

 //           PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, payloadLayer);

 //           return builder.Build(DateTime.Now);
 //       }

        

 //       /// <summary>
 //       /// This function build an UDP over IPv4 over Ethernet with payload packet.
 //       /// </summary>
 //       private static Packet BuildUdpPacket()
 //       {
 //           EthernetLayer ethernetLayer =
 //               new EthernetLayer
 //                   {
 //                       Source = new MacAddress("01:01:01:01:01:01"),
 //                       Destination = new MacAddress("02:02:02:02:02:02"),
 //                       EtherType = EthernetType.None, // Will be filled automatically.
 //                   };

 //           IpV4Layer ipV4Layer =
 //               new IpV4Layer
 //                   {
 //                       Source = new IpV4Address("1.2.3.4"),
 //                       CurrentDestination = new IpV4Address("11.22.33.44"),
 //                       Fragmentation = IpV4Fragmentation.None,
 //                       HeaderChecksum = null, // Will be filled automatically.
 //                       Identification = 123,
 //                       Options = IpV4Options.None,
 //                       Protocol = null, // Will be filled automatically.
 //                       Ttl = 100,
 //                       TypeOfService = 0,
 //                   };

 //           UdpLayer udpLayer =
 //               new UdpLayer
 //                   {
 //                       SourcePort = 4050,
 //                       DestinationPort = 25,
 //                       Checksum = null, // Will be filled automatically.
 //                       CalculateChecksumValue = true,
 //                   };

 //           PayloadLayer payloadLayer =
 //               new PayloadLayer
 //                   {
 //                       Data = new Datagram(Encoding.ASCII.GetBytes("hello world")),
 //                   };

 //           PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, udpLayer, payloadLayer);

 //           return builder.Build(DateTime.Now);
 //       }

 //       /// <summary>
 //       /// This function build an TCP over IPv4 over Ethernet with payload packet.
 //       /// </summary>
 //       private static Packet BuildTcpPacket()
 //       {
 //           EthernetLayer ethernetLayer =
 //               new EthernetLayer
 //                   {
 //                       Source = new MacAddress("01:01:01:01:01:01"),
 //                       Destination = new MacAddress("02:02:02:02:02:02"),
 //                       EtherType = EthernetType.None, // Will be filled automatically.
 //                   };

 //           IpV4Layer ipV4Layer =
 //               new IpV4Layer
 //                   {
 //                       Source = new IpV4Address("1.2.3.4"),
 //                       CurrentDestination = new IpV4Address("11.22.33.44"),
 //                       Fragmentation = IpV4Fragmentation.None,
 //                       HeaderChecksum = null, // Will be filled automatically.
 //                       Identification = 123,
 //                       Options = IpV4Options.None,
 //                       Protocol = null, // Will be filled automatically.
 //                       Ttl = 100,
 //                       TypeOfService = 0,
 //                   };

 //           TcpLayer tcpLayer =
 //               new TcpLayer
 //                   {
 //                       SourcePort = 4050,
 //                       DestinationPort = 25,
 //                       Checksum = null, // Will be filled automatically.
 //                       SequenceNumber = 100,
 //                       AcknowledgmentNumber = 50,
 //                       ControlBits = TcpControlBits.Acknowledgment,
 //                       Window = 100,
 //                       UrgentPointer = 0,
 //                       Options = TcpOptions.None,
 //                   };

 //           PayloadLayer payloadLayer =
 //               new PayloadLayer
 //                   {
 //                       Data = new Datagram(Encoding.ASCII.GetBytes("hello world")),
 //                   };

 //           PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, tcpLayer, payloadLayer);

 //           return builder.Build(DateTime.Now);
 //       }
        
