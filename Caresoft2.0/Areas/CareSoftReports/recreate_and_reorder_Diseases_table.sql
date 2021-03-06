
USE [CaresoftHMIS]
GO

drop table Diseases
GO

/****** Object:  Table [dbo].[Diseases]    Script Date: 9/13/2018 12:12:22 PM ******/
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
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (5, N'Menigococcal Meningitis', 5, 5, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.603' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (6, N'Other Meningitis', 6, 6, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.657' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (7, N'Neonatal Tetanus', 7, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.707' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (8, N'Tetanus', 0, 7, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.767' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (9, N'Poliomyelitis (AFP)', 8, 8, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.820' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (10, N'Chicken pox', 9, 9, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.873' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (11, N'Typhoid Fever', 17, 17, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.923' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (12, N'Hepatitis', 11, 11, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:12.987' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (13, N'Mumps', 12, 12, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.043' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (14, N'Fevers', 13, 13, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.097' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (15, N'Suspected Malaria', 14, 14, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.153' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (16, N'Confirmed Malaria', 15, 15, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.207' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (17, N'Malaria in pregnancy', 0, 16, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.260' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (18, N'Urinary Tract Infection', 16, 19, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.317' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (20, N'Bilharzia', 18, 20, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.423' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (21, N'Interstinal Worms', 19, 21, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.480' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (22, N'Malnutrion', 20, 22, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.533' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (23, N'Neonatal Deaths', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.590' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (24, N'Eye Infections', 22, 24, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.667' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (25, N'Other Eye conditions', 23, 25, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.740' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (26, N'Ear Infections / Conditions', 24, 26, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.790' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (27, N'Upper Respiratory Tract infections', 25, 27, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.847' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (28, N'Asthma', 26, 28, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.897' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (29, N'Suspected MDR/XDR TB', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:13.950' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (30, N'Pneumonia', 28, 29, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.003' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (31, N'Other Dis. of Respiratory System', 29, 30, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.053' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (32, N'Mental Disorders', 30, 32, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.110' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (33, N'Violence Related Injuries', 51, 43, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.163' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (34, N'Dis. Of puerperium & Childbirth', 0, 33, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.220' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (35, N'Hypertension', 0, 34, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.290' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (36, N'Dental Disorders', 31, 35, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.360' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (37, N'Jiggers infection', 32, 36, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.433' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (38, N'Diseases of the Skin', 33, 37, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.487' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (39, N'Chromosmal abnormalities (eg. downs, Edwards syndromes ,etc)', 34, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.557' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (40, N'Congential Anomalies', 35, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.630' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (41, N'Antritis ,Joint pains , etc', 0, 38, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.690' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (42, N'poisoning', 36, 39, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.747' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (43, N'Road Traffic Injuries', 37, 40, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.800' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (44, N'Other Injuries', 38, 41, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.857' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (45, N'Sexual Assault', 39, 42, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:14.930' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (46, N'Burns', 40, 44, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.000' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (48, N'Snake bites', 41, 45, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.107' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (49, N'Dog Bites', 42, 46, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.160' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (50, N'Other Bites', 43, 47, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.243' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (51, N'Diabetes', 44, 48, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.320' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (52, N'Epilepsy', 45, 49, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.383' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (53, N'Newly Diagnosed HIV', 47, 50, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.450' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (54, N'Other Convulsive Disorders', 46, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.510' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (55, N'Brucellosis', 48, 51, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.567' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (56, N'Cardiovascular Conditions', 50, 52, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.647' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (57, N'Rickets', 49, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.730' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (58, N'Central Nervous System', 0, 53, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.787' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (59, N'Overweight (BMI>25)', 0, 54, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.847' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (60, N'Muscular skeletal Conditions', 0, 55, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.907' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (61, N'Fistula (Birth Related)', 0, 56, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:15.967' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (62, N'Neoplams', 0, 57, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.043' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (63, N'Physical Disability', 0, 58, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.130' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (64, N'Cerebral palsy', 52, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.193' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (65, N'Autism', 53, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.253' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (66, N'Tryponosomiasis', 55, 59, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.340' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (67, N'Kalazar (Leishmaniaisis)', 56, 60, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.410' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (68, N'Dracunclulosis (Guinea Worm)', 57, 61, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.470' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (69, N'Viral Haemorrhagic Fever', 59, 63, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.553' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (70, N'Dengue', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.637' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (71, N'Malaria', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.697' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (72, N'yellow fever', 58, 62, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.753' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (73, N'other diseases', 0, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.817' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (74, N'SARI (Cluster >3 cases)', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.873' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (75, N'Rift Valley Fever', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.933' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (76, N'VHF***', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:16.997' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (77, N'Rabies', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.070' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (78, N'Maternal Deaths', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.173' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (79, N'Other Central Nervous System Conditions', 54, 0, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.230' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (80, N'Plague', 60, 64, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.287' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (81, N'Deaths due to Road Traffic Injuries', 61, 65, 0, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.340' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (82, N'AFP (Poliomyelitis)**', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.393' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (83, N'Anthrax', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.447' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (84, N'Guinea Worm Disease (Dracunculiasis)', 0, 0, 1, 0, 0, 1, 1, CAST(N'2018-08-31T20:08:17.500' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (87, N'Measles', 10, 10, 1, 0, 0, 1, 1, CAST(N'2018-09-10T10:08:49.553' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (88, N'Aneamia', 21, 23, 0, 0, 0, 1, 1, CAST(N'2018-09-10T00:00:00.000' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (89, N'Tonsillities', 27, 0, 0, 0, 0, 1, 1, CAST(N'2018-09-10T00:00:00.000' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (94, N'sexually transmitted infections', 0, 18, 0, 0, 0, 1, 1, CAST(N'2018-09-10T00:00:00.000' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (95, N'Arbotion', 0, 31, 0, 0, 0, 1, 1, CAST(N'2018-09-10T00:00:00.000' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (96, N'AEFI*', NULL, NULL, 1, NULL, NULL, 1, 1, CAST(N'2018-09-11T11:14:16.117' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (97, N'Acute Jaundice', NULL, NULL, 1, NULL, NULL, 1, 1, CAST(N'2018-09-11T11:14:23.067' AS DateTime))
INSERT [dbo].[Diseases] ([Id], [DiseaseName], [CodeMOH705_A], [CodeMOH705_B], [EpidemicFlag], [STIFlag], [CHANISFlag], [UserId], [BranchId], [TimeAdded]) VALUES (98, N'Acute Malnutrition', NULL, NULL, 1, NULL, NULL, 1, 1, CAST(N'2018-09-11T11:14:30.180' AS DateTime))
SET IDENTITY_INSERT [dbo].[Diseases] OFF
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_EpidemicFlag]  DEFAULT ((0)) FOR [EpidemicFlag]
GO
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_STIFlag]  DEFAULT ((0)) FOR [STIFlag]
GO
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_CHANISFlag]  DEFAULT ((0)) FOR [CHANISFlag]
GO
ALTER TABLE [dbo].[Diseases] ADD  CONSTRAINT [DF_Diseases_TimeAdded]  DEFAULT (getdate()) FOR [TimeAdded]
GO
