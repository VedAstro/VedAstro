import SwiftUI

struct ServerOutputView: View {
    
    @Binding var textOutput: String // success outputs into this
    @Binding var errorOutput: String // errors output into this
    
    var body: some View {
        VStack{
            // label above
            HStack {
                Text("🖥 Server Output")
                   .font(.footnote)
                   .fontWeight(.light)
                
                Spacer()
            }
            ScrollView() {
                VStack {
                    if !errorOutput.isEmpty {
                        Text("Error: \(errorOutput)")
                           .multilineTextAlignment(.leading)
                           .lineLimit(nil)
                           .padding([.top,.leading], 8.0) // places text in top right corner
                           .disabled(false) // make it read-only
                           .frame(maxWidth:.infinity, alignment:.topLeading) // fill available width
                           .frame(minHeight: 412, alignment:.topLeading) // will increase
                           .background(Color.red.opacity(0.8)) // error background color
                           .foregroundColor(.white) // error text color
                           .cornerRadius(14) // rounded corners
                           .overlay(
                                RoundedRectangle(cornerRadius: 14)
                                   .stroke(Color.red, lineWidth: 3) // error border
                            )
                    } else {
                        Text(textOutput)
                           .multilineTextAlignment(.leading)
                           .lineLimit(nil)
                           .padding([.top,.leading], 8.0) // places text in top right corner
                           .disabled(false) // make it read-only
                           .frame(maxWidth:.infinity, alignment:.topLeading) // fill available width
                           .frame(minHeight: 412, alignment:.topLeading) // will increase
                           .background(Color.black.opacity(0.8)) // terminal background color
                           .foregroundColor(.green) // terminal text color
                           .cornerRadius(14) // rounded corners
                           .overlay(
                                RoundedRectangle(cornerRadius: 14)
                                   .stroke(Color.gray, lineWidth: 3) // terminal border
                            )
                    }
                }
            }
        }
       .padding(.trailing, 8)
       .padding(.top, 4)
    }
}

struct ServerOutputView_Previews: PreviewProvider {
    static var previews: some View {
        ServerOutputView(textOutput:.constant("Server launching good..."), errorOutput:.constant(""))
           .previewLayout(.sizeThatFits) // adjust preview size to fit content
    }
}
