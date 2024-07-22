import SwiftUI

struct ContentView: View {
    @State private var serverOutput = ""
    @State private var errorOutput = ""
    
    var body: some View {
        HStack {
            AppHeaderView(serverOutput: $serverOutput, errorOutput: $errorOutput) //buttons
            Divider()
            ServerOutputView(textOutput: $serverOutput, errorOutput: $errorOutput) //output
        }
        .frame(minWidth: 546, minHeight: 509)
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
