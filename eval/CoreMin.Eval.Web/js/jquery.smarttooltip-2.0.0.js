/*
 * jQuery SmartTooltip 2.0.0
 *
 * Copyright 2010, Andreas Börcsök
 * Licensed under the GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://smartexpert.de/docs/jQuery-SmartTooltip
 *
 * Depends:
 *	jquery-1.7.1.js
 */
(function ($, jQuery) {

	var settings; //global to this function
	var isDocked = false; //global to this function
	var $tooltip;

	jQuery.fn.smarttooltip = function (callerSettings) {

		settings = $.extend({}, jQuery.fn.smarttooltip.defaults, callerSettings);

		return initSmartTooltip(this);
	}; // end of smarttooltip function

	jQuery.fn.smarttooltip.defaults = {
		// tooltip container id
		tooltipContainerId: "smartTooltip",
		// additional x and y offset from mouse cursor for tooltips
		tooltipOffsets: [10, 7],
		// duration of fade in effect in milliseconds
		fadeInSpeed: 200,
		// sticky tooltip when user right clicks over the triggering element (apart from pressing "s" key)
		rightClickStick: true,
		//border color of tooltip depending on tooltip state
		//tooltipBorderColors: ['#b7babf', '#b7babf'],
		//status text color of tooltip depending on tooltip state
		//statusTextColors: ['#d06026', '#509020'],
		statusTextColors: ['#509020', '#509020'],
		// tooltip status message in hover status
		hoverStatusText: ["Dr&uuml;cke \"s\"", "oder rechts-click", "f&uuml;r Daueranzeige."],
		// tooltip status message in sticky status
		stickyStatusText: "Click ausserhalb um Hinweis zu schliessen."
	};


	var initSmartTooltip = function ($targetSelector) {
		$tooltip = $('#' + settings.tooltipContainerId);

		if ($targetSelector.length == 0)
			return;

		var allTips = $tooltip.find('.smartTip');

		$targetSelector.each(function () {
			$(this).data('tipref', $(this).attr('title'));
			$(this).attr('title', '');
		});

		if (!settings.rightClickStick)
			settings.hoverStatusText[1] = '';

		settings.hoverStatusText = settings.hoverStatusText.join(' ');

		hideBox();

		$targetSelector.bind('mouseenter', function (e) {
			allTips.hide().filter('#' + $(this).data('tipref')).show();
			showBox(e);
		});

		$targetSelector.bind('mouseleave', function (e) {
			hideBox();
		});

		$targetSelector.bind('mousemove', function (e) {
			if (!isDocked) {
				positionTooltip(e);
			}
		});

		$tooltip.bind("mouseenter", function () {
			hideBox();
		});

		$tooltip.bind("click", function (e) {
			e.stopPropagation();
		});

		$(document).bind("click", function (e) {
			if (e.button == 0) {
				isDocked = false;
				hideBox();
			}
		});

		$(document).bind("contextmenu", function (e) {
			if (settings.rightClickStick && $(e.target).parents().andSelf().filter($targetSelector).length == 1) {
				//if oncontextmenu over a target element
				dockTooltip(e);
				return false;
			}
			return true;
		});

		$(document).bind('keypress', function (e) {
			var keyunicode = e.charCode || e.keyCode;

			if (keyunicode == 115) {
				//if "s" key was pressed
				dockTooltip(e);
			}
		});

	};

	var showBox = function (e) {
		$tooltip.fadeIn(settings.fadeInSpeed);
		positionTooltip(e);
	};

	var hideBox = function () {
		if (!isDocked) {
			$tooltip.stop(false, true).hide();
			$tooltip //.css({ 'border-color': settings.tooltipBorderColors[0] })
				.find('.smartStatus:eq(0)')
				.css({
					//"border-color": settings.tooltipBorderColors[0],
					"color": settings.statusTextColors[0]
				})
				.html(settings.hoverStatusText);
		}
	};

	var dockTooltip = function (e) {
		isDocked = true;
		$tooltip //.css({ "border-color": settings.tooltipBorderColors[1] })
			.find('.smartStatus:eq(0)')
			.css({
				//"border-color": settings.tooltipBorderColors[1],
				"color": settings.statusTextColors[1]
			})
			.html(settings.stickyStatusText);
	};

	var positionTooltip = function (e) {
		var xOffset = settings.tooltipOffsets[0];
		var yOffset = settings.tooltipOffsets[1];

		var x = e.pageX + xOffset;
		var y = e.pageY + yOffset;

		var tipWidth = $tooltip.outerWidth();
		var tipHeight = $tooltip.outerHeight();


		x = (x + tipWidth > $(document).scrollLeft() + $(window).width()) ?
			x - tipWidth - (xOffset * 2) : x;
		if (x < 0) { x = 0; }

		y = (y + tipHeight > $(document).scrollTop() + $(window).height()) ?
			$(document).scrollTop() + $(window).height() - tipHeight - yOffset : y;

		$tooltip.css({ left: x, top: y });
	};

})(jQuery, jQuery);