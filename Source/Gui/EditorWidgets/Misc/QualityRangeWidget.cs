using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class QualityRangeWidget : IDefEditorWidget
	{
		private QualityRange QualityRange;

		private FloatOptionsArgs<QualityCategory> MinQualityRange;
		private FloatOptionsArgs<QualityCategory> MaxQualityRange;

		public QualityRangeWidget(QualityRange qr)
		{
			this.QualityRange = qr;

			this.ResetBuffers();
		}

		public string DisplayLabel => "Quality Range";

		public void Draw(float x, ref float y, float width)
		{
			WindowUtil.DrawLabel(x, y, width, this.DisplayLabel, true);
			y += 40;

			x += 10;
			WindowUtil.DrawLabel(x, y, 100, "Min");
			if (Widgets.ButtonText(new Rect(110f, y, 100, 30), this.QualityRange.min.GetLabel()))
				WindowUtil.DrawFloatingOptions(this.MinQualityRange);
			y += 40;

			WindowUtil.DrawLabel(x, y, 100, "Max");
			if (Widgets.ButtonText(new Rect(110f, y, 100, 30), this.QualityRange.max.GetLabel()))
				WindowUtil.DrawFloatingOptions(this.MaxQualityRange);
			y += 40;
		}

		public void ResetBuffers()
		{
			IEnumerable<QualityCategory> categories = Enum.GetValues(typeof(QualityCategory)).Cast<QualityCategory>().ToList();
			this.MinQualityRange = new FloatOptionsArgs<QualityCategory>()
			{
				items = categories,
				getDisplayName = (category) => category.GetLabel(),
				includeNullOption = false,
				onSelect = (category) => this.QualityRange.min = category
			};
			this.MaxQualityRange = new FloatOptionsArgs<QualityCategory>()
			{
				items = categories,
				getDisplayName = (category) => category.GetLabel(),
				includeNullOption = false,
				onSelect = (category) => this.QualityRange.max = category
			};
		}
	}
}
