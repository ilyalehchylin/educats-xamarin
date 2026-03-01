using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Pages.SaveLabsAndPracticeMarks.ViewModels;
using NUnit.Framework;

namespace EduCATS.UnitTests.Pages
{
	[TestFixture]
	public class SaveSingleStudentMarkPageViewModelTests
	{
		[Test]
		public void SetMarksLoadsLabMarkCommentAndVisibilityByLabId()
		{
			var viewModel = createWithoutConstructor();
			viewModel.studentName = "Student A";
			viewModel._subGruop = 2;
			viewModel._takedLabs = new TakedLabs {
				Labs = new List<TakedLab> {
					new TakedLab { LabId = 20, ShortName = "Lab 2", SubGroup = 2 }
				}
			};
			viewModel.fullMarksLabs = new LabsVisitingList {
				Students = new List<LaboratoryWorksModel> {
					new LaboratoryWorksModel {
						FullName = "Student A",
						LabsMarks = new List<LabMarks> {
							new LabMarks {
								LabId = 10,
								Mark = "1",
								Comment = "wrong mark by index",
								ShowForStudent = false,
								Date = "28.02.2026"
							},
							new LabMarks {
								LabId = 20,
								Mark = "7",
								Comment = "Saved comment",
								ShowForStudent = true,
								Date = "01.03.2026"
							}
						}
					}
				}
			};

			viewModel.setMarks("Lab 2");

			Assert.AreEqual(7, viewModel.MarkStudent);
			Assert.AreEqual("Saved comment", viewModel.Comment);
			Assert.IsTrue(viewModel.ShowForStud);
			Assert.AreEqual("01.03.2026", viewModel.SelectedDate);
		}

		[Test]
		public void SetMarksResetsValuesWhenSelectionMissing()
		{
			var viewModel = createWithoutConstructor();
			viewModel.studentName = "Student A";
			viewModel._subGruop = 1;
			viewModel._takedLabs = new TakedLabs {
				Labs = new List<TakedLab> {
					new TakedLab { LabId = 10, ShortName = "Lab 1", SubGroup = 1 }
				}
			};
			viewModel.fullMarksLabs = new LabsVisitingList {
				Students = new List<LaboratoryWorksModel> {
					new LaboratoryWorksModel { FullName = "Student A" }
				}
			};

			viewModel.setMarks("Unknown Lab");

			Assert.AreEqual(0, viewModel.MarkStudent);
			Assert.AreEqual(string.Empty, viewModel.Comment);
			Assert.IsFalse(viewModel.ShowForStud);
			Assert.AreEqual(DateTime.Today.ToString("dd.MM.yyyy"), viewModel.SelectedDate);
		}

		static SaveSingleStudentMarkPageViewModel createWithoutConstructor()
		{
			return (SaveSingleStudentMarkPageViewModel)FormatterServices.GetUninitializedObject(
				typeof(SaveSingleStudentMarkPageViewModel));
		}
	}
}
