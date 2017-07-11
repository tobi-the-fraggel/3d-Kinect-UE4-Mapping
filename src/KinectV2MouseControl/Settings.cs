﻿using System;
using System.ComponentModel;

namespace Mousenect.Properties {
    
    
    // Diese Klasse ermöglicht die Behandlung bestimmter Ereignisse der Einstellungsklasse:
    //  Das SettingChanging-Ereignis wird ausgelöst, bevor der Wert einer Einstellung geändert wird.
    //  Das PropertyChanged-Ereignis wird ausgelöst, nachdem der Wert einer Einstellung geändert wurde.
    //  Das SettingsLoaded-Ereignis wird ausgelöst, nachdem die Einstellungswerte geladen wurden.
    //  Das SettingsSaving-Ereignis wird ausgelöst, bevor die Einstellungswerte gespeichert werden.
    internal sealed partial class Settings {
        
        public Settings() {
            this.PropertyChanged += this.PropertyChangedEventHandler;
        }

        private void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Fügen Sie hier Code zum Behandeln des SettingChangingEvent-Ereignisses hinzu.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Fügen Sie hier Code zum Behandeln des SettingsSaving-Ereignisses hinzu.
        }
    }
}
