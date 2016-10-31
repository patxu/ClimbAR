/**
 * Created by olykos on 10/16/16.
 *
 * Playing around with Background Processing
 *
 */

import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import javax.imageio.ImageIO;
import javax.imageio.ImageReader;
import javax.imageio.stream.ImageInputStream;

public class BackgroundProcessing {

    private static int MAX_DISTANCE = 1000;

    public static void main(String args[]) throws Exception {
        File file = new File("img.jpg");
        ImageInputStream is = ImageIO.createImageInputStream(file);
        Iterator iter = ImageIO.getImageReaders(is);

        if (!iter.hasNext())
        {
            System.out.println("Cannot load the specified file "+ file);
            System.exit(1);
        }
        ImageReader imageReader = (ImageReader)iter.next();
        imageReader.setInput(is);

        BufferedImage image = imageReader.read(0);

        int height = image.getHeight();
        int width = image.getWidth();

        Map m = new HashMap();
        for(int i=0; i < width ; i++)
        {
            for(int j=0; j < height ; j++)
            {
                int rgb = image.getRGB(i, j);
                Integer counter = (Integer) m.get(rgb);
                if (counter == null)
                    counter = 0;
                counter++;
                m.put(rgb, counter);
            }
        }
        int mostCommonColorInt = getMostCommonColor(m);
        Color mostCommonColor = new Color(mostCommonColorInt);

        //Second pass replacing colors

        Color myWhite = new Color(255, 255, 255); // Color white
        int white = myWhite.getRGB();

        for(int i=0; i < width ; i++)
        {
            for(int j=0; j < height ; j++)
            {
                int rgb = image.getRGB(i, j);
                if (isSimilarColor(new Color(rgb), mostCommonColor)) {
                    image.setRGB(i, j, white);
                }
            }
        }

        // retrieve image
        File outputfile = new File("saved.jpg");
        ImageIO.write(image, "jpg", outputfile);

        System.out.println(mostCommonColor);
    }


    private static Integer getMostCommonColor(Map map) {
        List list = new LinkedList(map.entrySet());
        Collections.sort(list, new Comparator() {
            public int compare(Object o1, Object o2) {
                return ((Comparable) ((Map.Entry) (o1)).getValue())
                        .compareTo(((Map.Entry) (o2)).getValue());
            }
        });
        Map.Entry me = (Map.Entry )list.get(list.size()-1);
//        int[] rgb= getRGBArr((Integer)me.getKey());
        return (Integer)me.getKey();
    }

//    private static int[] getRGBArr(int pixel) {
//        int alpha = (pixel >> 24) & 0xff;
//        int red = (pixel >> 16) & 0xff;
//        int green = (pixel >> 8) & 0xff;
//        int blue = (pixel) & 0xff;
//        return new int[]{red, green, blue};
//
//    }


    private static boolean isSimilarColor(Color a, Color b) {
        double distance = (a.getRed() - b.getRed())*(a.getRed() - b.getRed())
                        + (a.getGreen() - b.getGreen())*(a.getGreen() - b.getGreen())
                        + (a.getBlue() - b.getBlue())*(a.getBlue() - b.getBlue());
        return (distance < MAX_DISTANCE);
    }
}
