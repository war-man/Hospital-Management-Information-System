USE [CaresoftHMIS]
GO
/****** Object:  Table [dbo].[Diseases]    Script Date: 9/10/2018 5:47:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diseases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DiseaseName] [varchar](max) NOT NULL,
	[CodeMOH705_A] [int] NULL,
	[CodeMOH705_B] [int] NULL,
	[EpidemicFlag] [bit] NULL,
	[STIFlag] [bit] NULL,
	[CHANISFlag] [bit] NULL,
	[UserId] [int] NOT NULL,
	[BranchId] [int] NOT NULL,
	[TimeAdded] [datetime] NOT NULL,
 CONSTRAINT [PK_Diseases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Diseases] ON 

INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (1, N'Diarrhoea', 1, 1, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.360' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (2, N'Tuberculosis', 2, 2, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.417' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (3, N'Dysentry (blood diarrhoea , bacilillary)', 3, 3, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.497' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (4, N'Cholera', 4, 4, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.550' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (5, N'Menigococcal Meningitis', 5, 5, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.603' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (6, N'Other Meningitis', 6, 6, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.657' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (7, N'Neonatal Tetanus', 7, 7, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.707' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (8, N'Tetanus', 8, 8, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.767' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (9, N'Poliomyelitis (AFP)', 9, 9, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.820' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (10, N'Chicken pox', 10, 10, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.873' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (11, N'Typhoid', 11, 11, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.923' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (12, N'Hepatitis', 12, 12, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.987' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (13, N'Mumps', 13, 13, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.043' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (14, N'Fevers', 14, 14, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.097' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (15, N'Suspected Malaria', 15, 15, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.153' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (16, N'Confirmed Malaria', 16, 16, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.207' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (17, N'Malaria in pregnancy', 17, 17, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.260' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (18, N'Urunary Tract Infection', 18, 18, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.317' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (19, N'Meningococcal Meningitis', 19, 19, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.370' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (20, N'Bilharzia', 20, 20, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.423' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (21, N'Interstinal Worms', 21, 21, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.480' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (22, N'Malnutrion', 22, 22, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.533' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (23, N'Neonatal Deaths', 23, 23, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.590' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (24, N'eye conditions', 24, 24, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.667' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (25, N'Other Eye conditions', 25, 25, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.740' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (26, N'Ear Infections / Conditions', 26, 26, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.790' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (27, N'Upper Respiratory Tract infections', 27, 27, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.847' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (28, N'Asthma', 28, 28, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.897' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (29, N'Suspected MDR/XDR TB', 29, 29, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.950' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (30, N'Pneumonia', 30, 30, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.003' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (31, N'Other Dis. of Respiratory System', 31, 31, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.053' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (32, N'Mental Disorders', 32, 32, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.110' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (33, N'Violence Related Injuries', 33, 33, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.163' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (34, N'Dis. Of puerperium & Childbirth', 34, 34, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.220' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (35, N'Hypertension', 35, 35, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.290' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (36, N'Dental Disorders', 36, 36, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.360' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (37, N'Jiggers infection', 37, 37, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.433' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (38, N'Diseases of the Skin', 38, 38, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.487' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (39, N'Chromosmal abnormalities (eg. downs, Edwards syndromes ,etc)', 39, 39, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.557' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (40, N'Congential Anomalies', 40, 40, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.630' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (41, N'Antritis ,Joint pains , etc', 41, 41, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.690' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (42, N'poisoning', 42, 42, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.747' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (43, N'Road Traffic Injuries', 43, 43, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.800' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (44, N'Other Injuries', 44, 44, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.857' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (45, N'Sexual Assault', 45, 45, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.930' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (46, N'Burns', 46, 46, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.000' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (47, N'Violance Related Injuries', 47, 47, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.053' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (48, N'Snake bites', 48, 48, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.107' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (49, N'Dog Bites', 49, 49, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.160' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (50, N'Other Bites', 50, 50, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.243' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (51, N'Diabetes', 51, 51, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.320' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (52, N'Epilepsy', 52, 52, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.383' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (53, N'Newly Diagnosed HIV', 53, 53, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.450' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (54, N'Other Convulsive Disorders', 54, 54, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.510' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (55, N'Brucellosis', 55, 55, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.567' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (56, N'Cardiovascular Conditions', 56, 56, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.647' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (57, N'Rickets', 57, 57, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.730' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (58, N'Central Nervous System', 58, 58, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.787' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (59, N'Overweight (BMI>25)', 59, 59, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.847' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (60, N'Muscular skeletal Conditions', 60, 60, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.907' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (61, N'Fistula (Birth Related)', 61, 61, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.967' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (62, N'Neoplams', 62, 62, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.043' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (63, N'Physical Disability', 63, 63, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.130' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (64, N'Cerebral palsy', 64, 64, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.193' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (65, N'Autism', 65, 65, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.253' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (66, N'Tryponosomiasis', 66, 66, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.340' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (67, N'Kalazar (Leishmaniaisis)', 67, 67, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.410' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (68, N'Dracunclulosis (Guinea Worm)', 68, 68, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.470' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (69, N'Viral Haemorrhagic Fever', 69, 69, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.553' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (70, N'Dengue', 70, 70, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.637' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (71, N'Malaria', 71, 71, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.697' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (72, N'yellow fever', 72, 72, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.753' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (73, N'other diseases', 73, 73, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.817' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (74, N'SARI (Cluster >3 cases)', 74, 74, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.873' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (75, N'Rift Valley Fever', 75, 75, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.933' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (76, N'VHF***', 76, 76, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.997' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (77, N'Rabies', 77, 77, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.070' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (78, N'Maternal Deaths', 78, 78, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.173' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (79, N'Other Central Nervous System Conditions', 79, 79, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.230' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (80, N'Plague', 80, 80, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.287' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (81, N'Deaths due to Road Traffic Injuries', 81, 81, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.340' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (82, N'AFP (Poliomyelitis)**', 82, 82, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.393' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (83, N'Anthrax', 83, 83, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.447' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (84, N'Guinea Worm Disease (Dracunculiasis)', 84, 84, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.500' AS DateTime))
SET IDENTITY_INSERT [dbo].[Diseases] OFF
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_EpidemicFlag]  DEFAULT ((0)) FOR [EpidemicFlag]
GO
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_STIFlag]  DEFAULT ((0)) FOR [STIFlag]
GO
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_CHANISFlag]  DEFAULT ((0)) FOR [CHANISFlag]
GO
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_TimeAdded]  DEFAULT (getdate()) FOR [TimeAdded]
GO
