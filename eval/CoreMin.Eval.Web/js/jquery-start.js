$(document).ready(function () {

	Modernizr.load({
		test: Modernizr.flexboxlegacy,
		nope: '/js/vendor/polyfills/flexie-1.0.3.min.js'
	});

    /*$("#sidebar .nav a").each(function () {
		var paddingLeftOrg = $(this).css('paddingLeft');
		$(this).hover(
			function () {
				$(this).animate({ paddingLeft: '+=12px' }, 200, 'swing');
				$(this).queue(function () {
					$(this).addClass('navHoverBackground');
					$(this).dequeue();
				});
			},
			function () {
				$(this).stop(true, true);
				$(this).removeClass('navHoverBackground');
				$(this).animate({ paddingLeft: paddingLeftOrg }, 200, 'swing');
			}
		);
	});

	$.get("tooltips.html", function (data) {
		$("#smartTooltipData").append(data);
		$('.smartNote').smarttooltip();
	});*/


});

//$(window).resize(function (e) {
//	document.title = "width: " + window.innerWidth + " | " + window.outerWidth;
//	//window.console.log("width: " + window.innerWidth + " | " + window.outerWidth);
//});


