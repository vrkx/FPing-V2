 // --- APP STATE & CONFIG ---
    let appSettings = {};
    let translations = {};
    let pingIntervalId = null;
    const appVersion = '2.0.1';



    function startPingInterval() {
        if (pingIntervalId) clearInterval(pingIntervalId);
        if (appSettings.pingInterval > 0) {
            pingIntervalId = setInterval(pingAllServers, appSettings.pingInterval);
        }
    }

    startPingInterval();
    export default startPingInterval;